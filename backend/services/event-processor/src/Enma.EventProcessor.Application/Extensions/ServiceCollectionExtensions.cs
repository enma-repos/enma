using Enma.EventProcessor.Application.Abstractions;
using Enma.EventProcessor.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.EventProcessor.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventBatchProcessor, EventBatchProcessor>();
        return services;
    }
}
