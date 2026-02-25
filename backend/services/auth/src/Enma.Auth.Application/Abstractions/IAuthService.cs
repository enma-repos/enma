using Enma.Auth.Application.Dto.External;
using Enma.Auth.Application.Dto.Responses;
using FluentResults;

namespace Enma.Auth.Application.Abstractions;

public interface IAuthService
{
    Task<Result<GetAccountResponseDto>> AuthenticateExternalAsync(ExternalAuthRequestDto request, CancellationToken ct = default);
}
