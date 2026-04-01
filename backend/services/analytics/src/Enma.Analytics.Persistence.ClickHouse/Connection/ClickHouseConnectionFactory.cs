using System.Data.Common;
using ClickHouse.Driver.ADO;
using Enma.Common.Constants;
using Enma.Common.Options;
using Microsoft.Extensions.Options;

namespace Enma.Analytics.Persistence.ClickHouse.Connection;

internal sealed class ClickHouseConnectionFactory : IClickHouseConnectionFactory
{
    private readonly string _connectionString;

    public string Database { get; }
    public int CommandTimeoutSeconds { get; }

    public ClickHouseConnectionFactory(IOptions<ClickHouseOptions> options)
    {
        var value = options.Value;
        var parsedConnectionString = new DbConnectionStringBuilder
        {
            ConnectionString = value.ConnectionString
        };

        if (!TryReadDatabase(parsedConnectionString, out var database))
        {
            throw new InvalidOperationException("ClickHouse connection string must contain 'Database'.");
        }

        if (!RegexPatterns.ClickHouseTableIdentifier().IsMatch(database))
        {
            throw new InvalidOperationException($"Invalid ClickHouse database name '{database}'.");
        }

        _connectionString = value.ConnectionString;
        Database = database;
        CommandTimeoutSeconds = value.CommandTimeoutSeconds;
    }

    public async Task<DbConnection> OpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = new ClickHouseConnection(_connectionString);
        await connection.OpenAsync(ct);
        return connection;
    }

    private static bool TryReadDatabase(DbConnectionStringBuilder connectionStringBuilder, out string database)
    {
        database = string.Empty;

        if (!connectionStringBuilder.TryGetValue("Database", out var databaseValue) || databaseValue is null)
        {
            return false;
        }

        database = databaseValue.ToString() ?? string.Empty;
        return !string.IsNullOrWhiteSpace(database);
    }
}
