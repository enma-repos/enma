using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.BucketBuilder.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWindowBucketBuildService, WindowBucketBuildService>();
        services.AddScoped<IPathAggregationService, PathAggregationService>();
        services.AddScoped<IChainStitchingService, ChainStitchingService>();

        return services;
    }
}
