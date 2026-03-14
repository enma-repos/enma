using Enma.EventProcessor.Application.Extensions;
using Enma.EventProcessor.Infrastructure.RabbitMq.Extensions;
using Enma.EventProcessor.Persistence.ClickHouse.Extensions;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("service", "enma-event-processor")
    .CreateLogger();

builder.Services.AddSerilog(Log.Logger, dispose: true);

builder.Services
    .AddApplication(builder.Configuration)
    .AddRabbitMqMessaging(builder.Configuration)
    .AddClickHousePersistence(builder.Configuration);

var host = builder.Build();
await host.RunAsync();