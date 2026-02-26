using FluentResults;

namespace Enma.Auth.Application.Contracts.Infrastructure.Caching;

public interface ICacheService
{
    Task<Result<T>> GetAsync<T>(string key);
    Task<Result<bool>> AddAsync<T>(string key, T value, int expiresInMinutes = 60);
    Task<Result<bool>> RemoveAsync(string key);
    Task<Result<bool>> ReplaceAsync<T>(string key, T value, int expiresInMinutes = 60);
}