using Enma.Auth.Infrastructure.ExternalAuth.Google.Dto;
using FluentResults;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Abstractions;

internal interface IGoogleIdTokenValidator
{
    Task<Result<GoogleIdTokenClaimsDto>> ValidateAsync(string idToken, CancellationToken ct);
}