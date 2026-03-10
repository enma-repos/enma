using Enma.Common.Errors;
using FluentResults;

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
    public string EventName { get; }
    public DateTime OccurredAtUtc { get; }
    public string? ActorUserId { get; }
    public string? ActorAnonymousId { get; }

    private PathSourceEvent(
        Guid eventId,
        ChainKey chainKey,
        string eventName,
        DateTime occurredAtUtc,
        string? actorUserId,
        string? actorAnonymousId)
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

        ModelValidation.AddRequiredGuid(errors, eventId, nameof(eventId));

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        var normalizedEventName = ModelValidation.ValidateRequiredString(
            errors,
            eventName,
            nameof(eventName),
            minLength: 1,
            maxLength: 200);

        ModelValidation.AddUtcDateTime(errors, occurredAtUtc, nameof(occurredAtUtc));

        var normalizedActorUserId = ModelValidation.ValidateOptionalString(
            errors,
            actorUserId,
            nameof(actorUserId),
            maxLength: 256);

        var normalizedActorAnonymousId = ModelValidation.ValidateOptionalString(
            errors,
            actorAnonymousId,
            nameof(actorAnonymousId),
            maxLength: 256);

        return errors.Count > 0
            ? Result.Fail<PathSourceEvent>(errors)
            : Result.Ok(new PathSourceEvent(
                eventId,
                chainKey!,
                normalizedEventName,
                occurredAtUtc,
                normalizedActorUserId,
                normalizedActorAnonymousId));
    }

    public static PathSourceEvent Rehydrate(
        Guid eventId,
        ChainKey chainKey,
        string eventName,
        DateTime occurredAtUtc,
        string? actorUserId,
        string? actorAnonymousId)
        => new(eventId, chainKey, eventName, occurredAtUtc, actorUserId, actorAnonymousId);
}