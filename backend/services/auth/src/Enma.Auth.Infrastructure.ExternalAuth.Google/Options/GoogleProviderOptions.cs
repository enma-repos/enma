namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Options;

public sealed record GoogleProviderOptions
{
    public required string BaseAddress { get; init; }
    public int TimeoutSeconds { get; init; }

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string RedirectUrl { get; init; }
    
    public bool RequireEmailVerified { get; init; } = true;
    public int ClockSkewSeconds { get; init; } = 60;
}