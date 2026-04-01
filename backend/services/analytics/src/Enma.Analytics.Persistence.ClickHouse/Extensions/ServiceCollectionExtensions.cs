using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Persistence.ClickHouse.Connection;
using Enma.Analytics.Persistence.ClickHouse.Repositories;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Analytics.Persistence.ClickHouse.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClickHousePersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<ClickHouseOptions>()
            .Bind(configuration.GetSection("ClickHouse"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "ClickHouse:ConnectionString is required.")
            .Validate(o => o.CommandTimeoutSeconds > 0,
                "ClickHouse:CommandTimeoutSeconds must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton<IClickHouseConnectionFactory, ClickHouseConnectionFactory>();
        services.AddScoped<IUniqueUsersQueryRepository, UniqueUsersQueryRepository>();

        return services;
    }
}
