using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Analytics.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IFlowGraphService, FlowGraphService>();
        services.AddScoped<IFunnelAnalysisService, FunnelAnalysisService>();
        services.AddScoped<ITopEventsService, TopEventsService>();
        services.AddScoped<IEntryExitPointsService, EntryExitPointsService>();
        services.AddScoped<ITimeTrendsService, TimeTrendsService>();
        services.AddScoped<IEventDetailService, EventDetailService>();
        services.AddScoped<IActorBreakdownService, ActorBreakdownService>();
        services.AddScoped<ISummaryService, SummaryService>();

        return services;
    }
}
