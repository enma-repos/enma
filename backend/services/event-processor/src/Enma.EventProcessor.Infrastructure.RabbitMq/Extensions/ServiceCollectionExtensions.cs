using Enma.Common.Options;
using Enma.EventProcessor.Infrastructure.RabbitMq.Consumers;
using Enma.EventProcessor.Infrastructure.RabbitMq.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.EventProcessor.Infrastructure.RabbitMq.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.Configure<EventProcessorRabbitMqOptions>(configuration.GetSection("EventProcessor:RabbitMq"));

        var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()
                              ?? throw new InvalidOperationException(
                                  "RabbitMq section is missing. Set 'RabbitMq:ConnectionString'.");

        if (string.IsNullOrWhiteSpace(rabbitMqOptions.ConnectionString))
        {
            throw new InvalidOperationException(
                "RabbitMq connection string is missing. Set 'RabbitMq:ConnectionString'.");
        }

        var consumerOptions = configuration
            .GetRequiredSection("EventProcessor:RabbitMq")
            .Get<EventProcessorRabbitMqOptions>()!;

        services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();

            x.AddConsumer<EventBatchConsumer>(cfg =>
            {
                cfg.Options<BatchOptions>(options => options
                    .SetMessageLimit(consumerOptions.BatchMessageLimit)
                    .SetTimeLimit(TimeSpan.FromSeconds(consumerOptions.BatchTimeLimitSeconds))
                    .SetConcurrencyLimit(consumerOptions.BatchConcurrencyLimit));
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqOptions.ConnectionString));
                cfg.UseDelayedMessageScheduler();

                cfg.ReceiveEndpoint(consumerOptions.EndpointName, endpoint =>
                {
                    endpoint.PrefetchCount = consumerOptions.PrefetchCount;
                    endpoint.UseDelayedRedelivery(redelivery =>
                        redelivery.Intervals(BuildRedeliveryIntervals(consumerOptions).ToArray()));
                    endpoint.ConfigureConsumer<EventBatchConsumer>(context);
                });
            });
        });

        return services;
    }

    private static IEnumerable<TimeSpan> BuildRedeliveryIntervals(EventProcessorRabbitMqOptions options)
    {
        for (var attempt = 0; attempt < options.RetryCount; attempt++)
        {
            yield return TimeSpan.FromSeconds(options.RetryIntervalSeconds);
        }
    }
}
