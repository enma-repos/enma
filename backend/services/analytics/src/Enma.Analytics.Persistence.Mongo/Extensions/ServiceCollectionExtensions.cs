using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Persistence.Mongo.Connection;
using Enma.Analytics.Persistence.Mongo.Repositories;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Analytics.Persistence.Mongo.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<MongoDbOptions>()
            .Bind(configuration.GetSection("MongoDb"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "MongoDb:ConnectionString is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName),
                "MongoDb:DatabaseName is required.")
            .ValidateOnStart();

        services.AddSingleton<IMongoDbContext, MongoDbContext>();

        services.AddScoped<IPathNodeQueryRepository, PathNodeQueryRepository>();
        services.AddScoped<IPathEdgeQueryRepository, PathEdgeQueryRepository>();

        return services;
    }
}
