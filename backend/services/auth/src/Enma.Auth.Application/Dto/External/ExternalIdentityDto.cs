namespace Enma.Auth.Application.Dto.External;

public sealed record ExternalIdentityDto(
    string Provider,
    string Subject,
    string Email,
    bool EmailVerified,
    string? DisplayName,
    string? PictureUrl);