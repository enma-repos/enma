using System.Text.Json;
using Enma.ApiGateway.Sdk.Infrastructure.Caching.Abstractions;
using Enma.ApiGateway.Sdk.Infrastructure.Caching.Options;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Dto;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.ApiGateway.Sdk.Infrastructure.Caching.Services;

internal sealed class RedisSdkAuthCacheService : ISdkAuthCacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IDatabase _database;
    private readonly string _keyPrefix;
    private readonly int _cacheTtlMinutes;

    public RedisSdkAuthCacheService(
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisOptions> redisOptions,
        IOptions<SdkAuthCacheOptions> cacheOptions)
    {
        _database = connectionMultiplexer.GetDatabase(redisOptions.Value.Database);
        _keyPrefix = redisOptions.Value.KeyPrefix?.Trim() ?? string.Empty;
        _cacheTtlMinutes = cacheOptions.Value.CacheTtlMinutes;
    }

    public async Task<ApiKeyValidationResult?> GetAsync(string cacheKey)
    {
        var value = await _database.StringGetAsync(BuildKey(cacheKey));
        if (!value.HasValue)
            return null;

        try
        {
            return JsonSerializer.Deserialize<ApiKeyValidationResult>(value!, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task SetAsync(string cacheKey, ApiKeyValidationResult result)
    {
        var json = JsonSerializer.Serialize(result, JsonOptions);
        await _database.StringSetAsync(
            BuildKey(cacheKey),
            json,
            expiry: TimeSpan.FromMinutes(_cacheTtlMinutes));
    }

    private RedisKey BuildKey(string key)
        => string.IsNullOrWhiteSpace(_keyPrefix)
            ? $"sdk-auth:{key}"
            : $"{_keyPrefix}:sdk-auth:{key}";
}
