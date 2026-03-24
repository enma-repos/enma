using Enma.Common.Errors;
using FluentResults;
using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Last known event in a chain; enables stitching transitions across bucket boundaries.
/// </summary>
public sealed class ChainCursor
{
    public ChainKey ChainKey { get; }
    public Guid LastEventId { get; }
    public EventName LastEventName { get; }
    public DateTime LastOccurredAtUtc { get; }
    public ActorIdentifier? LastActorUserId { get; }
    public ActorIdentifier? LastActorAnonymousId { get; }
    public DateTime UpdatedAtUtc { get; }
    public EventName FirstEventName { get; }

    private ChainCursor(
        ChainKey chainKey,
        Guid lastEventId,
        EventName lastEventName,
        DateTime lastOccurredAtUtc,
        ActorIdentifier? lastActorUserId,
        ActorIdentifier? lastActorAnonymousId,
        DateTime updatedAtUtc,
        EventName firstEventName)
    {
        ChainKey = chainKey;
        LastEventId = lastEventId;
        LastEventName = lastEventName;
        LastOccurredAtUtc = lastOccurredAtUtc;
        LastActorUserId = lastActorUserId;
        LastActorAnonymousId = lastActorAnonymousId;
        UpdatedAtUtc = updatedAtUtc;
        FirstEventName = firstEventName;
    }

    public static Result<ChainCursor> Create(
        ChainKey? chainKey,
        Guid lastEventId,
        string? lastEventName,
        DateTime lastOccurredAtUtc,
        string? lastActorUserId,
        string? lastActorAnonymousId,
        DateTime updatedAtUtc,
        string? firstEventName)
    {
        var errors = new List<IError>();

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        if (lastEventId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(lastEventId)));
        }

        var lastEventNameVoResult = EventName.Create(lastEventName);
        if (lastEventNameVoResult.IsFailed)
        {
            errors.AddRange(lastEventNameVoResult.Errors);
        }

        var firstEventNameVoResult = EventName.Create(firstEventName);
        if (firstEventNameVoResult.IsFailed)
        {
            errors.AddRange(firstEventNameVoResult.Errors);
        }

        if (lastOccurredAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(lastOccurredAtUtc)} must be UTC."));
        }

        if (updatedAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(updatedAtUtc)} must be UTC."));
        }

        if (updatedAtUtc < lastOccurredAtUtc)
        {
            errors.Add(ApplicationErrors.Validation("updatedAtUtc cannot be earlier than lastOccurredAtUtc."));
        }

        var lastActorUserIdVoResult = ActorIdentifier.CreateOptional(lastActorUserId);
        if (lastActorUserIdVoResult.IsFailed)
        {
            errors.AddRange(lastActorUserIdVoResult.Errors);
        }

        var lastActorAnonymousIdVoResult = ActorIdentifier.CreateOptional(lastActorAnonymousId);
        if (lastActorAnonymousIdVoResult.IsFailed)
        {
            errors.AddRange(lastActorAnonymousIdVoResult.Errors);
        }

        return errors.Count > 0
            ? Result.Fail<ChainCursor>(errors)
            : Result.Ok(new ChainCursor(
                chainKey!,
                lastEventId,
                lastEventNameVoResult.Value,
                lastOccurredAtUtc,
                lastActorUserIdVoResult.Value,
                lastActorAnonymousIdVoResult.Value,
                updatedAtUtc,
                firstEventNameVoResult.Value));
    }

    public static ChainCursor Rehydrate(
        ChainKey chainKey,
        Guid lastEventId,
        string lastEventName,
        DateTime lastOccurredAtUtc,
        string? lastActorUserId,
        string? lastActorAnonymousId,
        DateTime updatedAtUtc,
        string firstEventName)
        => new(
            chainKey,
            lastEventId,
            EventName.Rehydrate(lastEventName),
            lastOccurredAtUtc,
            string.IsNullOrWhiteSpace(lastActorUserId) ? null : ActorIdentifier.Rehydrate(lastActorUserId),
            string.IsNullOrWhiteSpace(lastActorAnonymousId) ? null : ActorIdentifier.Rehydrate(lastActorAnonymousId),
            updatedAtUtc,
            EventName.Rehydrate(firstEventName));
}
