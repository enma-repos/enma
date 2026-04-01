using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Dto;

namespace Enma.ApiGateway.Sdk.Infrastructure.Caching.Abstractions;

public interface ISdkAuthCacheService
{
    Task<ApiKeyValidationResult?> GetAsync(string cacheKey);
    Task SetAsync(string cacheKey, ApiKeyValidationResult result);
}
