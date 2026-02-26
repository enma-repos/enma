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
        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
        
        return services;
    }
}
