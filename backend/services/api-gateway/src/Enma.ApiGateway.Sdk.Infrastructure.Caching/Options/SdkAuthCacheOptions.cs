namespace Enma.ApiGateway.Sdk.Infrastructure.Caching.Options;

public sealed record SdkAuthCacheOptions
{
    public int CacheTtlMinutes { get; init; } = 5;
}
