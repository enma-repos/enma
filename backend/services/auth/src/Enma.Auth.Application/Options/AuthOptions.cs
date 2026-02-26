namespace Enma.Auth.Application.Options;

public sealed record AuthOptions
{
    public int RefreshTokenLifetimeDays { get; init; }
    public int StateCacheTtlMinutes { get; init; }
    public required string RedirectBaseAddress { get; init; }
}