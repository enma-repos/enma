namespace Enma.ApiGateway.Sdk.Infrastructure.Caching.Options;

public sealed record RedisOptions
{
    public required string ConnectionString { get; init; }
    public string? KeyPrefix { get; init; }
    public int Database { get; init; }
}
