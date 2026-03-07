using Enma.EventProcessor.Application.Extensions;
using Enma.EventProcessor.Infrastructure.RabbitMq.Extensions;
using Enma.EventProcessor.Persistence.ClickHouse.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
    .AddRabbitMqMessaging(builder.Configuration)
    .AddClickHousePersistence(builder.Configuration);

var host = builder.Build();
await host.RunAsync();