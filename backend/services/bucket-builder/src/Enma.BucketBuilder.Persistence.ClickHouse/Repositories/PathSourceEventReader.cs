using System.Data.Common;
using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.BucketBuilder.Persistence.ClickHouse.Connection;
using FluentResults;

namespace Enma.BucketBuilder.Persistence.ClickHouse.Repositories;

internal sealed class PathSourceEventReader : IPathSourceEventReader
{
    private readonly IClickHouseConnectionFactory _connectionFactory;

    public PathSourceEventReader(IClickHouseConnectionFactory connectionFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionFactory);
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<IReadOnlyCollection<PathSourceEvent>>> GetWindowAsync(
        BucketWindow window,
        CancellationToken ct = default)
    {
        await using var connection = await _connectionFactory.OpenConnectionAsync(ct);
        await using var command = connection.CreateCommand();

        command.CommandTimeout = _connectionFactory.CommandTimeoutSeconds;
        command.CommandText = """
            SELECT
                e.event_id,
                e.organization_id,
                e.project_id,
                pk_def_id,
                pk_id,
                e.event_name,
                e.occurred_at,
                e.actor_user_id,
                e.actor_anonymous_id
            FROM events AS e
            ARRAY JOIN
                e.`process_keys.process_definition_id` AS pk_def_id,
                e.`process_keys.process_id` AS pk_id
            WHERE e.occurred_at >= {start:DateTime64(3, 'UTC')}
              AND e.occurred_at < {end:DateTime64(3, 'UTC')}
            ORDER BY e.organization_id, e.project_id, pk_def_id, pk_id, e.occurred_at
            """;

        AddParameter(command, "start", window.StartUtc.Value);
        AddParameter(command, "end", window.EndUtc.Value);

        await using var reader = await command.ExecuteReaderAsync(ct);

        var results = new List<PathSourceEvent>(1024);

        while (await reader.ReadAsync(ct))
        {
            var eventId = reader.GetGuid(0);
            var organizationId = reader.GetGuid(1);
            var projectId = reader.GetGuid(2);
            var processDefinitionId = reader.GetGuid(3);
            var processId = reader.GetString(4);
            var eventName = reader.GetString(5);
            var occurredAt = DateTime.SpecifyKind(reader.GetDateTime(6), DateTimeKind.Utc);
            var actorUserId = reader.IsDBNull(7) ? null : reader.GetString(7);
            var actorAnonymousId = reader.IsDBNull(8) ? null : reader.GetString(8);

            var chainKey = ChainKey.Rehydrate(organizationId, projectId, processDefinitionId, processId);
            var sourceEvent = PathSourceEvent.Rehydrate(
                eventId, chainKey, eventName, occurredAt, actorUserId, actorAnonymousId);

            results.Add(sourceEvent);
        }

        return Result.Ok<IReadOnlyCollection<PathSourceEvent>>(results);
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
