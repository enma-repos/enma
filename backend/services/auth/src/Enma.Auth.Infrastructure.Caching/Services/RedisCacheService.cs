using Enma.Auth.Application.Contracts.Infrastructure.Caching;
using Enma.Common.Errors;
using FluentResults;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Enma.Auth.Infrastructure.Caching.Services;

internal sealed class RedisCacheService : ICacheService
{
    private readonly IRedisDatabase _database;

    public RedisCacheService(IRedisDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<T>> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<T>(ApplicationErrors.Required(nameof(key)));
        }

        var result = await _database.GetAsync<T>(key);
        return result is null 
            ? Result.Fail<T>(ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found")) 
            : Result.Ok(result);
    }

    public async Task<Result<bool>> AddAsync<T>(string key, T value, int expiresInMinutes = 60)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<bool>(ApplicationErrors.Required(nameof(key)));
        }

        var resultSuccess = await _database.AddAsync(key, value, expiresIn: TimeSpan.FromMinutes(expiresInMinutes));

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

        var resultSuccess = await _database.RemoveAsync(key);
        
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

        var resultSuccess = 
            await _database.ReplaceAsync(key, value, expiresIn: TimeSpan.FromMinutes(expiresInMinutes));
        
        return resultSuccess 
            ? Result.Ok(resultSuccess) 
            : Result.Fail<bool>(ApplicationErrors.NotFound($"Cache key '{key}' does not exist.", code: "cache_key_not_found"));
    }
}
