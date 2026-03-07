using Enma.Common.Options;
using Enma.EventProcessor.Persistence.ClickHouse.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enma.EventProcessor.Persistence.ClickHouse.Services;

internal sealed class ClickHouseMigrationHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ClickHouseOptions _options;

    public ClickHouseMigrationHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<ClickHouseOptions> options)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        if (!_options.ApplyMigrationsOnStartup)
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IClickHouseSchemaMigrator>();
        await migrator.MigrateAsync(ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
