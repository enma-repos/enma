namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Dto;

internal sealed record GoogleIdTokenClaimsDto(
    string Subject,
    string Email,
    bool EmailVerified,
    string? Name,
    string? PictureUrl);