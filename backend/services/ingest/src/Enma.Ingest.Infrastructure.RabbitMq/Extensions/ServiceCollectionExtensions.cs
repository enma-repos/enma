using Enma.Common.Options;
using Enma.Ingest.Application.Contracts.Infrastructure.Messaging;
using Enma.Ingest.Infrastructure.RabbitMq.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Ingest.Infrastructure.RabbitMq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("RabbitMq");
        var uri = section["ConnectionString"]
                  ?? throw new InvalidOperationException(
                      "RabbitMq connection string is missing. Set 'RabbitMq:ConnectionString'");

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(uri));
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
        return services;
    }
}
