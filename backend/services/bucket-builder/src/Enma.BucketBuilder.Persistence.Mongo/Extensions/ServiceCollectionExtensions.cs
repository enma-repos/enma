using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.JobsOrchestration.Contracts;
using Enma.BucketBuilder.Persistence.Mongo.Connection;
using Enma.BucketBuilder.Persistence.Mongo.Repositories;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.BucketBuilder.Persistence.Mongo.Extensions;

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

        services.AddScoped<IPathNodeBucketRepository, PathNodeBucketRepository>();
        services.AddScoped<IPathEdgeBucketRepository, PathEdgeBucketRepository>();
        services.AddScoped<IChainCursorRepository, ChainCursorRepository>();
        services.AddScoped<IShardCheckpointRepository, ShardCheckpointRepository>();

        return services;
    }
}
