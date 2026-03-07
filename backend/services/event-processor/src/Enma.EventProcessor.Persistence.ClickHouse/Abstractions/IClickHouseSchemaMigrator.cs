namespace Enma.EventProcessor.Persistence.ClickHouse.Abstractions;

public interface IClickHouseSchemaMigrator
{
    Task MigrateAsync(CancellationToken ct = default);
}
