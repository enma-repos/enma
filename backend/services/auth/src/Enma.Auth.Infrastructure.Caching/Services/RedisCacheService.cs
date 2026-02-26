using System.Text.Json;
using Enma.Auth.Application.Contracts.Infrastructure.Caching;
using Enma.Common.Errors;
using Enma.Common.Options;
using FluentResults;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.Auth.Infrastructure.Caching.Services;

internal sealed class RedisCacheService : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IDatabase _database;
    private readonly string _keyPrefix;

    public RedisCacheService(
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisOptions> options)
    {
        _database = connectionMultiplexer.GetDatabase(options.Value.Database);
        _keyPrefix = options.Value.KeyPrefix?.Trim() ?? string.Empty;
    }

    private RedisKey BuildKey(string key)
        => string.IsNullOrWhiteSpace(_keyPrefix) ? key : $"{_keyPrefix}:{key}";
    
    public async Task<Result<T>> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<T>(ApplicationErrors.Required(nameof(key)));
        }

        var value = await _database.StringGetAsync(BuildKey(key));
        if (!value.HasValue)
        {
            return Result.Fail<T>(
                ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found"));
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(value!, JsonOptions);
            return result is null
                ? Result.Fail<T>(ApplicationErrors.InvariantViolation("Cache value deserialized to null."))
                : Result.Ok(result);
        }
        catch (JsonException)
        {
            return Result.Fail<T>(ApplicationErrors.InvariantViolation("Failed to deserialize cache value."));
        }
    }

    public async Task<Result<T>> GetDelAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<T>(ApplicationErrors.Required(nameof(key)));
        }

        var value = await _database.StringGetDeleteAsync(BuildKey(key));
        if (!value.HasValue)
        {
            return Result.Fail<T>(
                ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found"));
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(value!, JsonOptions);
            return result is null
                ? Result.Fail<T>(ApplicationErrors.InvariantViolation("Cache value deserialized to null."))
                : Result.Ok(result);
        }
        catch (JsonException)
        {
            return Result.Fail<T>(ApplicationErrors.InvariantViolation("Failed to deserialize cache value."));
        }
    }

    public async Task<Result<bool>> AddAsync<T>(string key, T value, int expiresInMinutes = 60)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<bool>(ApplicationErrors.Required(nameof(key)));
        }

        var json = JsonSerializer.Serialize(value, JsonOptions);
        var resultSuccess = await _database.StringSetAsync(
            BuildKey(key),
            json,
            expiry: TimeSpan.FromMinutes(expiresInMinutes),
            when: When.NotExists);

        return resultSuccess 
            ? Result.Ok(resultSuccess) 
            : Result.Fail<bool>(ApplicationErrors.Conflict($"Cache key '{key}' already exists.", code: "cache_key_exists"));
    }

    public async Task<Result<bool>> RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<bool>(ApplicationErrors.Required(nameof(key)));
        }

        var resultSuccess = await _database.KeyDeleteAsync(BuildKey(key));
        
        return resultSuccess 
            ? Result.Ok(resultSuccess) 
            : Result.Fail<bool>(ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found"));
    }

    public async Task<Result<bool>> ReplaceAsync<T>(string key, T value, int expiresInMinutes = 60)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<bool>(ApplicationErrors.Required(nameof(key)));
        }

        var json = JsonSerializer.Serialize(value, JsonOptions);
        var resultSuccess = await _database.StringSetAsync(
            BuildKey(key),
            json,
            expiry: TimeSpan.FromMinutes(expiresInMinutes),
            when: When.Exists);
        
        return resultSuccess 
            ? Result.Ok(resultSuccess) 
            : Result.Fail<bool>(ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found"));
    }
}
