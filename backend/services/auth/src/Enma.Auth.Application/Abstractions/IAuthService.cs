using Enma.Auth.Application.Dto.Auth;
using FluentResults;

namespace Enma.Auth.Application.Abstractions;

public interface IAuthService
{
    Task<Result<string>> GetProviderUrlAsync(StartExternalAuthRequestDto dto);
    Task<Result<(AuthTokensDto AuthTokens, string SuccessUrl)>> AuthenticateExternalAsync(ExternalAuthCallbackDto dto, CancellationToken ct = default);
    Task<Result<AuthTokensDto>> RefreshAsync(RefreshTokensDto dto, CancellationToken ct = default);
    Task<Result> LogoutAsync(LogoutDto dto, CancellationToken ct = default);
    Task<Result<MeDto>> GetMeAsync(Guid accountId, CancellationToken ct = default);
    Task<Result<CompleteOnboardingResultDto>> CompleteOnboardingAsync(Guid accountId, CompleteOnboardingDto dto, CancellationToken ct = default);
}
