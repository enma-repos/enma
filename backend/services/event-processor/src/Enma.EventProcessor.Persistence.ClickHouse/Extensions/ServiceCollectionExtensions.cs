using ClickHouse.Driver;
using Enma.Common.Options;
using Enma.EventProcessor.Application.Contracts.Persistence.ClickHouse;
using Enma.EventProcessor.Persistence.ClickHouse.Abstractions;
using Enma.EventProcessor.Persistence.ClickHouse.Connection;
using Enma.EventProcessor.Persistence.ClickHouse.Repositories;
using Enma.EventProcessor.Persistence.ClickHouse.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.EventProcessor.Persistence.ClickHouse.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClickHousePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ClickHouseOptions>()
            .Bind(configuration.GetSection("ClickHouse"))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
                "ClickHouse:ConnectionString is required.")
            .Validate(options => options.CommandTimeoutSeconds > 0,
                "ClickHouse:CommandTimeoutSeconds must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ClickHouseOptions>>().Value;
            return new ClickHouseClient(options.ConnectionString);
        });

        services.AddSingleton<IClickHouseConnectionFactory, ClickHouseConnectionFactory>();
        services.AddScoped<IClickHouseSchemaMigrator, ClickHouseSchemaMigrator>();
        services.AddScoped<IEventsRepository, ClickHouseEventsRepository>();
        services.AddHostedService<ClickHouseMigrationHostedService>();

        return services;
    }
}
