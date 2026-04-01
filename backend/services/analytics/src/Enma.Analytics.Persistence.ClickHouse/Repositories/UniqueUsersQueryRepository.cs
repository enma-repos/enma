using System.Data.Common;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using Enma.Analytics.Persistence.ClickHouse.Connection;
using FluentResults;

namespace Enma.Analytics.Persistence.ClickHouse.Repositories;

internal sealed class UniqueUsersQueryRepository(
    IClickHouseConnectionFactory connectionFactory) : IUniqueUsersQueryRepository
{
    public async Task<Result<UniqueUsersCount>> GetUniqueUsersAsync(
        Guid organizationId,
        Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange,
        CancellationToken ct = default)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(ct);
        await using var command = connection.CreateCommand();

        command.CommandTimeout = connectionFactory.CommandTimeoutSeconds;

        if (processDefinitionIds is null or { Count: 0 })
        {
            command.CommandText = """
                SELECT
                    uniq(actor_user_id)      AS unique_users,
                    uniq(actor_anonymous_id)  AS unique_anonymous
                FROM events
                WHERE organization_id = {orgId:UUID}
                  AND project_id = {projId:UUID}
                  AND occurred_at >= {from:DateTime64(3, 'UTC')}
                  AND occurred_at < {to:DateTime64(3, 'UTC')}
                """;
        }
        else
        {
            command.CommandText = """
                SELECT
                    uniq(e.actor_user_id)      AS unique_users,
                    uniq(e.actor_anonymous_id)  AS unique_anonymous
                FROM events AS e
                ARRAY JOIN
                    e.`process_keys.process_definition_id` AS pk_def_id
                WHERE e.organization_id = {orgId:UUID}
                  AND e.project_id = {projId:UUID}
                  AND e.occurred_at >= {from:DateTime64(3, 'UTC')}
                  AND e.occurred_at < {to:DateTime64(3, 'UTC')}
                  AND pk_def_id IN ({defIds:Array(UUID)})
                """;

            AddParameter(command, "defIds", processDefinitionIds.ToArray());
        }

        AddParameter(command, "orgId", organizationId);
        AddParameter(command, "projId", projectId);
        AddParameter(command, "from", dateRange.FromUtc);
        AddParameter(command, "to", dateRange.ToUtc);

        await using var reader = await command.ExecuteReaderAsync(ct);

        if (await reader.ReadAsync(ct))
        {
            var uniqueUsers = reader.GetInt64(0);
            var uniqueAnonymous = reader.GetInt64(1);
            return Result.Ok(new UniqueUsersCount(uniqueUsers, uniqueAnonymous));
        }

        return Result.Ok(new UniqueUsersCount(0, 0));
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
