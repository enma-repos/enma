using System.Data.Common;

namespace Enma.Analytics.Persistence.ClickHouse.Connection;

public interface IClickHouseConnectionFactory
{
    string Database { get; }

    int CommandTimeoutSeconds { get; }

    Task<DbConnection> OpenConnectionAsync(CancellationToken ct = default);
}
