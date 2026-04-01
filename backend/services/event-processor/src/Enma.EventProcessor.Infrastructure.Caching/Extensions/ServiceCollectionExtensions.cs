using Enma.Common.Options;
using Enma.EventProcessor.Application.Contracts;
using Enma.EventProcessor.Infrastructure.Caching.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.EventProcessor.Infrastructure.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RedisOptions>()
            .Bind(configuration.GetSection("Redis"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "Redis:ConnectionString is required.")
            .ValidateOnStart();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        });

        services.AddSingleton<IEventDefinitionCacheService, RedisEventDefinitionCacheService>();

        return services;
    }
}
