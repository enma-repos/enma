using ClickHouse.Driver;
using Enma.Common.Models;
using Enma.EventProcessor.Application.Contracts.Persistence.ClickHouse;

namespace Enma.EventProcessor.Persistence.ClickHouse.Repositories;

internal sealed class ClickHouseEventsRepository : IEventsRepository
{
    private static readonly string[] InsertColumns =
    [
        "event_id",
        "organization_id",
        "project_id",
        "sdk_client_id",
        "event_name",
        "occurred_at",
        "ingested_at",
        "actor_user_id",
        "actor_anonymous_id",
        "process_keys.process_definition_id",
        "process_keys.process_id",
        "tags",
        "payload_json"
    ];

    private readonly ClickHouseClient _clickHouseClient;

    public ClickHouseEventsRepository(ClickHouseClient clickHouseClient)
    {
        _clickHouseClient = clickHouseClient;
    }

    public Task InsertBatchAsync(IReadOnlyCollection<EventMessageDto> events, CancellationToken ct = default)
    {
        return events.Count == 0 
            ? Task.CompletedTask : 
            _clickHouseClient.InsertBinaryAsync("events", InsertColumns, events.Select(MapRow), cancellationToken: ct);
    }

    private static object[] MapRow(EventMessageDto @event)
    {
        var processKeys = @event.ProcessKeys;

        return
        [
            @event.EventId,
            @event.OrganizationId,
            @event.ProjectId,
            @event.SdkClientId,
            @event.EventName,
            NormalizeUtcTimestamp(@event.OccurredAt),
            NormalizeUtcTimestamp(@event.IngestedAt),
            @event.Actor.UserId,
            @event.Actor.AnonymousId,
            processKeys.Select(key => key.ProcessDefinitionId).ToArray(),
            processKeys.Select(key => key.ProcessId).ToArray(),
            @event.Tags is null 
                ? new Dictionary<string, string>() 
                : new Dictionary<string, string>(@event.Tags),
            @event.Payload?.GetRawText() ?? "null"
        ];
    }

    private static DateTime NormalizeUtcTimestamp(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => value
        };
    }
}
