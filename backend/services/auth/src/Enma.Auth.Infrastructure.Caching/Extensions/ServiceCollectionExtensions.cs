using Enma.Auth.Application.Contracts.Infrastructure.Caching;
using Enma.Auth.Infrastructure.Caching.Services;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.Auth.Infrastructure.Caching.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<RedisOptions>()
            .Bind(configuration.GetSection("Redis"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "Redis:ConnectionString is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.KeyPrefix),
                "Redis:KeyPrefix is required.")
            .ValidateOnStart();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
        
        return services;
    }
}
