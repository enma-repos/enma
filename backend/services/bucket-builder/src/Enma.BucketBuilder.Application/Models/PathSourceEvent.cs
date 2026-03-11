using Enma.Common.Errors;
using FluentResults;
using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Normalized stream event used by the path aggregator.
/// One incoming telemetry event can produce multiple PathSourceEvent rows
/// if it has multiple process keys.
/// </summary>
public sealed class PathSourceEvent
{
    public Guid EventId { get; }
    public ChainKey ChainKey { get; }
    public EventName EventName { get; }
    public DateTime OccurredAtUtc { get; }
    public ActorIdentifier? ActorUserId { get; }
    public ActorIdentifier? ActorAnonymousId { get; }

    private PathSourceEvent(
        Guid eventId,
        ChainKey chainKey,
        EventName eventName,
        DateTime occurredAtUtc,
        ActorIdentifier? actorUserId,
        ActorIdentifier? actorAnonymousId)
    {
        EventId = eventId;
        ChainKey = chainKey;
        EventName = eventName;
        OccurredAtUtc = occurredAtUtc;
        ActorUserId = actorUserId;
        ActorAnonymousId = actorAnonymousId;
    }

    public static Result<PathSourceEvent> Create(
        Guid eventId,
        ChainKey? chainKey,
        string? eventName,
        DateTime occurredAtUtc,
        string? actorUserId,
        string? actorAnonymousId)
    {
        var errors = new List<IError>();

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        if (eventId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(eventId)));
        }

        var eventNameVoResult = EventName.Create(eventName);
        if (eventNameVoResult.IsFailed)
        {
            errors.AddRange(eventNameVoResult.Errors);
        }

        if (occurredAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(occurredAtUtc)} must be UTC."));
        }

        var actorUserIdVoResult = ActorIdentifier.CreateOptional(actorUserId);
        if (actorUserIdVoResult.IsFailed)
        {
            errors.AddRange(actorUserIdVoResult.Errors);
        }

        var actorAnonymousIdVoResult = ActorIdentifier.CreateOptional(actorAnonymousId);
        if (actorAnonymousIdVoResult.IsFailed)
        {
            errors.AddRange(actorAnonymousIdVoResult.Errors);
        }

        return errors.Count > 0
            ? Result.Fail<PathSourceEvent>(errors)
            : Result.Ok(new PathSourceEvent(
                eventId,
                chainKey!,
                eventNameVoResult.Value,
                occurredAtUtc,
                actorUserIdVoResult.Value,
                actorAnonymousIdVoResult.Value));
    }

    public static PathSourceEvent Rehydrate(
        Guid eventId,
        ChainKey chainKey,
        string eventName,
        DateTime occurredAtUtc,
        string? actorUserId,
        string? actorAnonymousId)
        => new(
            eventId,
            chainKey,
            EventName.Rehydrate(eventName),
            occurredAtUtc,
            string.IsNullOrWhiteSpace(actorUserId) ? null : ActorIdentifier.Rehydrate(actorUserId),
            string.IsNullOrWhiteSpace(actorAnonymousId) ? null : ActorIdentifier.Rehydrate(actorAnonymousId));
}
