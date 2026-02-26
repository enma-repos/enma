using Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;
using Enma.Auth.Application.Dto;
using Enma.Auth.Application.Dto.External;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Abstractions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Http;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Options;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Services;

internal sealed class GoogleExternalIdentityProvider : IExternalIdentityProvider
{
    public string Name => "google";

    private readonly GoogleAuthClient _googleAuthClient;
    private readonly IGoogleIdTokenValidator _idTokenValidator;
    private readonly ILogger<GoogleExternalIdentityProvider> _logger;

    private readonly string _clientId;
    private readonly string _redirectUrl;

    public GoogleExternalIdentityProvider(
        GoogleAuthClient googleAuthClient,
        IGoogleIdTokenValidator idTokenValidator,
        IOptions<GoogleProviderOptions> options,
        ILogger<GoogleExternalIdentityProvider> logger)
    {
        _googleAuthClient = googleAuthClient;
        _idTokenValidator = idTokenValidator;
        _logger = logger;

        _clientId = options.Value.ClientId;
        _redirectUrl = options.Value.RedirectUrl;
    }

    public async Task<Result<ExternalIdentityDto>> AuthenticateAsync(
        ExternalAuthRequestDto request,
        CancellationToken ct)
    {
        var tokenResult = await _googleAuthClient.GetUserTokenAsync(request.Code, ct);
        if (tokenResult.IsFailed)
        {
            return Result.Fail<ExternalIdentityDto>(tokenResult.Errors);
        }

        var token = tokenResult.Value;
        if (string.IsNullOrWhiteSpace(token.IdToken))
        {
            return Result.Fail<ExternalIdentityDto>("Google token response missing id_token.");
        }

        var validationResult = await _idTokenValidator.ValidateAsync(token.IdToken!, ct);
        if (validationResult.IsFailed)
        {
            return Result.Fail<ExternalIdentityDto>(validationResult.Errors);
        }

        var identity = new ExternalIdentityDto(
            Provider: Name,
            Subject: validationResult.Value.Subject,
            Email: validationResult.Value.Email,
            EmailVerified: validationResult.Value.EmailVerified,
            DisplayName: validationResult.Value.Name,
            PictureUrl: validationResult.Value.PictureUrl
        );

        return Result.Ok(identity);
    }

    public string GetProviderUrl(Guid stateId) => $"https://accounts.google.com/o/oauth2/auth?" +
                                                  $"client_id={_clientId}&redirect_uri={_redirectUrl}&" +
                                                  $"response_type=code&scope=email%20profile&" +
                                                  $"state={stateId.ToString()}";
}