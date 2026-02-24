namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Dto;

internal sealed record GoogleUserInfoResponseDto
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string AvatarUrl { get; init; }
}