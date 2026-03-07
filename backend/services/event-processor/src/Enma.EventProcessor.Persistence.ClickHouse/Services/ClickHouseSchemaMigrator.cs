using System.Data.Common;
using Enma.Common.Constants;
using Enma.EventProcessor.Persistence.ClickHouse.Abstractions;
using Enma.EventProcessor.Persistence.ClickHouse.Connection;
using Microsoft.Extensions.Logging;

namespace Enma.EventProcessor.Persistence.ClickHouse.Services;

internal sealed class ClickHouseSchemaMigrator : IClickHouseSchemaMigrator
{
    private readonly IClickHouseConnectionFactory _connectionFactory;
    private readonly ILogger<ClickHouseSchemaMigrator> _logger;
    private readonly string _migrationsPath;

    public ClickHouseSchemaMigrator(
        IClickHouseConnectionFactory connectionFactory,
        ILogger<ClickHouseSchemaMigrator> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _migrationsPath = Path.Combine(AppContext.BaseDirectory, "Migrations");
    }

    public async Task MigrateAsync(CancellationToken ct = default)
    {
        await using var serverConnection = await _connectionFactory.OpenServerConnectionAsync(ct);
        await ExecuteAsync(
            serverConnection,
            $"CREATE DATABASE IF NOT EXISTS {_connectionFactory.Database}",
            _connectionFactory.CommandTimeoutSeconds,
            ct);

        await using var databaseConnection = await _connectionFactory.OpenConnectionAsync(ct);
        await ExecuteAsync(
            databaseConnection,
            """
            CREATE TABLE IF NOT EXISTS schema_migrations
            (
                version UInt32,
                name String,
                applied_at DateTime64(3, 'UTC') DEFAULT now64(3)
            )
            ENGINE = ReplacingMergeTree(applied_at)
            ORDER BY version
            """,
            _connectionFactory.CommandTimeoutSeconds,
            ct);

        var appliedVersions = await LoadAppliedVersionsAsync(databaseConnection, ct);

        foreach (var migration in DiscoverMigrations())
        {
            if (appliedVersions.Contains(migration.Version))
            {
                continue;
            }

            _logger.LogInformation("Applying ClickHouse migration {Version}_{Name}", migration.Version, migration.Name);

            var sql = await File.ReadAllTextAsync(migration.Path, ct);
            await ExecuteAsync(databaseConnection, sql, _connectionFactory.CommandTimeoutSeconds, ct);
            await ExecuteAsync(
                databaseConnection,
                $"INSERT INTO schema_migrations (version, name) VALUES ({migration.Version}, {ToSqlString(migration.Name)})",
                _connectionFactory.CommandTimeoutSeconds,
                ct);
        }
    }

    private async Task<HashSet<uint>> LoadAppliedVersionsAsync(DbConnection connection, CancellationToken ct)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT DISTINCT version FROM schema_migrations ORDER BY version";
        command.CommandTimeout = _connectionFactory.CommandTimeoutSeconds;

        await using var reader = await command.ExecuteReaderAsync(ct);
        var versions = new HashSet<uint>();

        while (await reader.ReadAsync(ct))
        {
            versions.Add(Convert.ToUInt32(reader.GetValue(0)));
        }

        return versions;
    }

    private IReadOnlyList<MigrationFile> DiscoverMigrations()
    {
        if (!Directory.Exists(_migrationsPath))
        {
            throw new DirectoryNotFoundException($"ClickHouse migrations directory was not found: {_migrationsPath}");
        }

        return Directory
            .EnumerateFiles(_migrationsPath, "*.sql", SearchOption.TopDirectoryOnly)
            .Select(filePath =>
            {
                var fileName = Path.GetFileName(filePath);
                var match = RegexPatterns.MigrationFileName().Match(fileName);

                if (!match.Success)
                {
                    throw new InvalidOperationException(
                        $"Invalid ClickHouse migration file name '{fileName}'. Expected '0001_name.sql'.");
                }

                return new MigrationFile(
                    uint.Parse(match.Groups["version"].Value),
                    match.Groups["name"].Value,
                    filePath);
            })
            .OrderBy(file => file.Version)
            .ThenBy(file => file.Name, StringComparer.Ordinal)
            .ToArray();
    }

    private static async Task ExecuteAsync(
        DbConnection connection,
        string sql,
        int commandTimeoutSeconds,
        CancellationToken ct)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandTimeout = commandTimeoutSeconds;

        await command.ExecuteNonQueryAsync(ct);
    }

    private static string ToSqlString(string value)
        => $"'{value.Replace("'", "''")}'";

    private sealed record MigrationFile(uint Version, string Name, string Path);
}
