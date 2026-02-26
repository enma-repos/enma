namespace Enma.Auth.Application.Options;

public sealed record AuthOptions
{
    public int RefreshTokenLifetimeDays { get; init; }
    public int TtlMinutes { get; init; }
}