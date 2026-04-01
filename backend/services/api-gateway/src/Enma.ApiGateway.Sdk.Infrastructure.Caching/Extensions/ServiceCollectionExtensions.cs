using Enma.ApiGateway.Sdk.Infrastructure.Caching.Abstractions;
using Enma.ApiGateway.Sdk.Infrastructure.Caching.Options;
using Enma.ApiGateway.Sdk.Infrastructure.Caching.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.ApiGateway.Sdk.Infrastructure.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSdkAuthCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RedisOptions>()
            .Bind(configuration.GetSection("Redis"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "Redis:ConnectionString is required.")
            .ValidateOnStart();

        services.AddOptions<SdkAuthCacheOptions>()
            .Bind(configuration.GetSection("SdkAuth"))
            .ValidateOnStart();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        });

        services.AddSingleton<ISdkAuthCacheService, RedisSdkAuthCacheService>();

        return services;
    }
}
