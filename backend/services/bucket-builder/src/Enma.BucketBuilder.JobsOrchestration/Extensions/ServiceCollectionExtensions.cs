using Enma.BucketBuilder.JobsOrchestration.Abstractions;
using Enma.BucketBuilder.JobsOrchestration.Options;
using Enma.BucketBuilder.JobsOrchestration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.BucketBuilder.JobsOrchestration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobsOrchestration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<BucketBuilderOptions>()
            .Bind(configuration.GetSection("BucketBuilder"))
            .Validate(o => o.PollIntervalSeconds > 0, "BucketBuilder:PollIntervalSeconds must be > 0.")
            .Validate(o => o.SafetyLagSeconds >= 0, "BucketBuilder:SafetyLagSeconds must be >= 0.")
            .Validate(o => o.MaxWindowsPerTick > 0, "BucketBuilder:MaxWindowsPerTick must be > 0.")
            .Validate(o => o.LeaseTimeoutSeconds > 0, "BucketBuilder:LeaseTimeoutSeconds must be > 0.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Pipeline), "BucketBuilder:Pipeline is required.")
            .Validate(o => o.ShardCount > 0, "BucketBuilder:ShardCount must be > 0.")
            .Validate(o => o.InitialLookbackHours > 0, "BucketBuilder:InitialLookbackHours must be > 0.")
            .ValidateOnStart();

        services.AddScoped<IBucketBuildingPipeline, BucketBuildingPipeline>();

        return services;
    }
}
