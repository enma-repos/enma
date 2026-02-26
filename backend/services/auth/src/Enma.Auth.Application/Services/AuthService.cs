using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Infrastructure.Caching;
using Enma.Auth.Application.Contracts.Infrastructure.Grpc.Admin;
using Enma.Auth.Application.Contracts.Infrastructure.Security;
using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Auth.Application.Dto.Auth;
using Enma.Auth.Application.Dto.Cache;
using Enma.Auth.Application.Dto.External;
using Enma.Auth.Application.Mapping;
using Enma.Auth.Application.Models;
using Enma.Auth.Application.Options;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Application.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IExternalAuthProviderFabric _externalAuthProviderFabric;
    private readonly IAccountsRepository _accountsRepository;
    private readonly IExternalAuthRepository _externalAuthRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly ICryptographyService _cryptographyService;
    private readonly IAdminUsersClient _adminUsersClient;
    private readonly ICacheService _cacheService;

    private readonly int _refreshTokenLifetimeDays;
    private readonly int _ttlMinutes;
    
    public AuthService(
        IExternalAuthProviderFabric externalAuthProviderFabric, 
        IAccountsRepository accountsRepository,
        IExternalAuthRepository externalAuthRepository,
        IRefreshTokensRepository refreshTokensRepository,
        IAccessTokenProvider accessTokenProvider,
        IAdminUsersClient adminUsersClient, 
        ICryptographyService cryptographyService, 
        ICacheService cacheService,
        IOptions<AuthOptions> authOptions)
    {
        _externalAuthProviderFabric = externalAuthProviderFabric;
        _accountsRepository = accountsRepository;
        _externalAuthRepository = externalAuthRepository;
        _refreshTokensRepository = refreshTokensRepository;
        _accessTokenProvider = accessTokenProvider;
        _adminUsersClient = adminUsersClient;
        _cryptographyService = cryptographyService;
        _cacheService = cacheService;

        _ttlMinutes = authOptions.Value.TtlMinutes;
        _refreshTokenLifetimeDays = authOptions.Value.RefreshTokenLifetimeDays;
    }

    public async Task<Result<string>> GetProviderUrlAsync(StartExternalAuthRequestDto dto)
    {
        if (!_externalAuthProviderFabric.TryGet(dto.Provider, out var provider))
        {
            return Result.Fail<string>(ApplicationErrors.Validation($"Unknown external auth provider '{dto.Provider}'."));
        }

        var stateId = Guid.NewGuid();
        var result = await _cacheService.AddAsync(stateId.ToString(), 
            new CachedStateDto(provider!.Name, dto.SuccessUrl), _ttlMinutes);
        
        return result.IsFailed 
            ? Result.Fail<string>(ApplicationErrors.Conflict("Cannot cache state.")) 
            : Result.Ok(provider.GetProviderUrl(stateId));
    }
    
    public async Task<Result<(AuthTokensDto AuthTokens, string SuccessUrl)>> AuthenticateExternalAsync(
        ExternalAuthCallbackDto dto,
        CancellationToken ct = default)
    {
        var stateResult = await _cacheService.GetAsync<CachedStateDto>(dto.State);
        if (stateResult.IsFailed)
        {
            return Result.Fail<(AuthTokensDto, string)>(ApplicationErrors.Validation("Invalid or expired state."));
        }
        
        if (!_externalAuthProviderFabric.TryGet(stateResult.Value.Provider, out var provider))
        {
            return Result.Fail<(AuthTokensDto, string)>(ApplicationErrors.Validation($"Unknown external auth provider '{stateResult.Value.Provider}'."));
        }

        var identityResult = await provider!.AuthenticateAsync(
            new ExternalAuthRequestDto(stateResult.Value.Provider, dto.Code),
            ct);

        if (identityResult.IsFailed)
        {
            return Result.Fail<(AuthTokensDto, string)>(identityResult.Errors);
        }

        var identity = identityResult.Value;

        var accountResult = await _externalAuthRepository.GetAccountByExternalAsync(identity.Provider, identity.Subject, ct);
        Account account;

        if (accountResult.IsSuccess)
        {
            account = accountResult.Value;
        }
        else 
        {
            var byEmailResult = await _accountsRepository.GetByEmailAsync(identity.Email, ct);
            if (byEmailResult.IsSuccess)
            {
                account = byEmailResult.Value;
            }
            else
            {
                var now = DateTime.UtcNow;

                var createModelResult = Account.Create(
                    id: Guid.NewGuid(),
                    email: identity.Email,
                    status: AccountStatus.PendingProfile,
                    passwordHash: null,
                    salt: null,
                    lastLoginAt: now,
                    onboardingStartedAt: now,
                    onboardingCompletedAt: null,
                    createdAt: now,
                    updatedAt: now,
                    deletedAt: null);

                if (createModelResult.IsFailed)
                {
                    return Result.Fail<(AuthTokensDto, string)>(createModelResult.Errors);
                }

                var createResult = await _accountsRepository.CreateAsync(createModelResult.Value, ct);
                if (createResult.IsFailed)
                {
                    return Result.Fail<(AuthTokensDto, string)>(createResult.Errors);
                }

                account = createModelResult.Value;
            }

            var linkedAt = DateTime.UtcNow;
            var externalAuthRes = ExternalAuth.Create(
                provider: identity.Provider,
                subject: identity.Subject,
                accountId: account.Id,
                linkedAt: linkedAt);

            if (externalAuthRes.IsFailed)
            {
                return Result.Fail<(AuthTokensDto, string)>(externalAuthRes.Errors);
            }

            var linkResult = await _externalAuthRepository.CreateAsync(externalAuthRes.Value, ct);
            if (linkResult.IsFailed)
            {
                return Result.Fail<(AuthTokensDto, string)>(linkResult.Errors);
            }
        }

        if (account.Status is AccountStatus.Banned or AccountStatus.Deleted)
        {
            return Result.Fail<(AuthTokensDto AuthTokens, string SuccessUrl)>(ApplicationErrors
                .Forbidden("Account is not allowed to authenticate."));
        }

        var updateAccResult = await _accountsRepository.UpdateLastLoginAsync(account.Id, ct);
        if (updateAccResult.IsFailed)
        {
            return Result.Fail<(AuthTokensDto, string)>(updateAccResult.Errors);
        }
        
        await _cacheService.RemoveAsync(dto.State);
        
        var tokensResult = await IssueTokensAsync(account, ct);
        return tokensResult.IsFailed 
            ? Result.Fail<(AuthTokensDto, string)>(tokensResult.Errors) 
            : Result.Ok((tokensResult.Value, stateResult.Value.SuccessUrl ?? string.Empty));
    }

    public async Task<Result<AuthTokensDto>> RefreshAsync(RefreshTokensDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
        {
            return Result.Fail<AuthTokensDto>(ApplicationErrors.Required(nameof(dto.RefreshToken)));
        }

        var tokenHash = _cryptographyService.ComputeSha256Hex(dto.RefreshToken);
        var tokenResult = await _refreshTokensRepository.GetByTokenHashAsync(tokenHash, ct);
        if (tokenResult.IsFailed)
        {
            return Result.Fail<AuthTokensDto>(ApplicationErrors.Validation("Invalid refresh token."));
        }

        var oldToken = tokenResult.Value;
        var now = DateTime.UtcNow;

        if (oldToken.ExpiresAt <= now)
        {
            return Result.Fail<AuthTokensDto>(ApplicationErrors.Validation("Refresh token expired."));
        }

        var accountResult = await _accountsRepository.GetByIdAsync(oldToken.AccountId, ct);
        if (accountResult.IsFailed)
        {
            return Result.Fail<AuthTokensDto>(accountResult.Errors);
        }

        var account = accountResult.Value;
        if (account.Status is AccountStatus.Banned or AccountStatus.Deleted)
        {
            return Result.Fail<AuthTokensDto>(ApplicationErrors
                .Forbidden("Account is not allowed to authenticate."));
        }

        var newTokenResult = await CreateRefreshTokenAsync(account.Id, now, ct);
        if (newTokenResult.IsFailed)
        {
            return Result.Fail<AuthTokensDto>(newTokenResult.Errors);
        }

        var deleteOldResult = await _refreshTokensRepository.DeleteAsync(oldToken.Id, ct);
        if (deleteOldResult.IsFailed)
        {
            return Result.Fail<AuthTokensDto>(deleteOldResult.Errors);
        }

        var accessToken = _accessTokenProvider.GenerateToken(account, newTokenResult.Value.TokenId);
        return Result.Ok(new AuthTokensDto(
            AccessToken: accessToken,
            RefreshToken: newTokenResult.Value.PlainToken,
            RefreshTokenExpiresAt: newTokenResult.Value.ExpiresAt));
    }

    public async Task<Result> LogoutAsync(LogoutDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
        {
            return Result.Fail(ApplicationErrors.Required(nameof(dto.RefreshToken)));
        }

        var tokenHash = _cryptographyService.ComputeSha256Hex(dto.RefreshToken);
        var tokenResult = await _refreshTokensRepository.GetByTokenHashAsync(tokenHash, ct);
        if (tokenResult.IsFailed)
        {
            return Result.Ok();
        }

        var deleteRes = await _refreshTokensRepository.DeleteAsync(tokenResult.Value.Id, ct);
        return deleteRes.IsSuccess ? Result.Ok() : Result.Fail(deleteRes.Errors);
    }

    public async Task<Result<MeDto>> GetMeAsync(Guid accountId, CancellationToken ct = default)
    {
        var accountResult = await _accountsRepository.GetByIdAsync(accountId, ct);
        if (accountResult.IsFailed)
        {
            return Result.Fail<MeDto>(accountResult.Errors);
        }
        
        var userResult = await _adminUsersClient.GetUserAsync(accountId, ct);
        if (userResult.IsFailed)
        {
            return Result.Fail<MeDto>(userResult.Errors);
        }

        return Result.Ok(new MeDto(
            Account: accountResult.Value.ToDto(),
            User: userResult.Value));
    }

    public async Task<Result<CompleteOnboardingResultDto>> CompleteOnboardingAsync(
        Guid accountId,
        CompleteOnboardingDto dto,
        CancellationToken ct = default)
    {
        var accountResult = await _accountsRepository.GetByIdAsync(accountId, ct);
        if (accountResult.IsFailed)
        {
            return Result.Fail<CompleteOnboardingResultDto>(accountResult.Errors);
        }

        var createUserResult = await _adminUsersClient.CreateUserAsync(
            new CreateAdminUserDto(
                AccountId: accountId,
                DisplayName: dto.DisplayName,
                AvatarUrl: dto.AvatarUrl,
                Locale: dto.Locale,
                Timezone: dto.Timezone),
            ct);

        if (createUserResult.IsFailed)
        {
            return Result.Fail<CompleteOnboardingResultDto>(createUserResult.Errors);
        }

        var now = DateTime.UtcNow;
        var completeResult = await _accountsRepository.CompleteOnboardingAsync(accountId, now, ct);
        if (completeResult.IsFailed)
        {
            return Result.Fail<CompleteOnboardingResultDto>(completeResult.Errors);
        }

        var updatedAccountResult = await _accountsRepository.GetByIdAsync(accountId, ct);
        if (updatedAccountResult.IsFailed)
        {
            return Result.Fail<CompleteOnboardingResultDto>(updatedAccountResult.Errors);
        }

        return Result.Ok(new CompleteOnboardingResultDto(
            Account: updatedAccountResult.Value.ToDto(),
            User: createUserResult.Value));
    }

    private async Task<Result<AuthTokensDto>> IssueTokensAsync(Account account, CancellationToken ct)
    {
        if (account.Status is AccountStatus.Banned or AccountStatus.Deleted)
        {
            return Result.Fail<AuthTokensDto>(ApplicationErrors.Forbidden("Account is not allowed to authenticate."));
        }

        var now = DateTime.UtcNow;

        var newTokenResult = await CreateRefreshTokenAsync(account.Id, now, ct);
        if (newTokenResult.IsFailed)
        {
            return Result.Fail<AuthTokensDto>(newTokenResult.Errors);
        }

        var accessToken = _accessTokenProvider.GenerateToken(account, newTokenResult.Value.TokenId);
        return Result.Ok(new AuthTokensDto(
            AccessToken: accessToken,
            RefreshToken: newTokenResult.Value.PlainToken,
            RefreshTokenExpiresAt: newTokenResult.Value.ExpiresAt));
    }

    private async Task<Result<(Guid TokenId, string PlainToken, DateTime ExpiresAt)>> CreateRefreshTokenAsync(
        Guid accountId,
        DateTime now,
        CancellationToken ct)
    {
        var plain = _cryptographyService.GenerateToken();
        var hash = _cryptographyService.ComputeSha256Hex(plain);

        var modelResult = RefreshToken.Create(
            id: Guid.NewGuid(),
            accountId: accountId,
            tokenHash: hash,
            createdAt: now,
            expiresAt: now.AddDays(_refreshTokenLifetimeDays),
            lastUsedAt: now);

        if (modelResult.IsFailed)
        {
            return Result.Fail<(Guid, string, DateTime)>(modelResult.Errors);
        }

        var createResult = await _refreshTokensRepository.CreateAsync(modelResult.Value, ct);
        return createResult.IsFailed 
            ? Result.Fail<(Guid, string, DateTime)>(createResult.Errors) 
            : Result.Ok((modelResult.Value.Id, plain, modelResult.Value.ExpiresAt));
    }
}
