namespace Enma.Auth.Application.Dto.Auth;

public sealed record AuthTokensDto(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);
