using System.Data.Common;

namespace Enma.EventProcessor.Persistence.ClickHouse.Connection;

public interface IClickHouseConnectionFactory
{
    string Database { get; }

    int CommandTimeoutSeconds { get; }

    Task<DbConnection> OpenConnectionAsync(CancellationToken ct = default);

    Task<DbConnection> OpenServerConnectionAsync(CancellationToken ct = default);
}
