using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Last known event in a chain; enables stitching transitions across bucket boundaries.
/// </summary>
public sealed class ChainCursor
{
    public ChainKey ChainKey { get; }
    public Guid LastEventId { get; }
    public string LastEventName { get; }
    public DateTime LastOccurredAtUtc { get; }
    public string? LastActorUserId { get; }
    public string? LastActorAnonymousId { get; }
    public DateTime UpdatedAtUtc { get; }

    private ChainCursor(
        ChainKey chainKey,
        Guid lastEventId,
        string lastEventName,
        DateTime lastOccurredAtUtc,
        string? lastActorUserId,
        string? lastActorAnonymousId,
        DateTime updatedAtUtc)
    {
        ChainKey = chainKey;
        LastEventId = lastEventId;
        LastEventName = lastEventName;
        LastOccurredAtUtc = lastOccurredAtUtc;
        LastActorUserId = lastActorUserId;
        LastActorAnonymousId = lastActorAnonymousId;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static Result<ChainCursor> Create(
        ChainKey? chainKey,
        Guid lastEventId,
        string? lastEventName,
        DateTime lastOccurredAtUtc,
        string? lastActorUserId,
        string? lastActorAnonymousId,
        DateTime updatedAtUtc)
    {
        var errors = new List<IError>();

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        ModelValidation.AddRequiredGuid(errors, lastEventId, nameof(lastEventId));

        var normalizedLastEventName = ModelValidation.ValidateRequiredString(
            errors,
            lastEventName,
            nameof(lastEventName),
            minLength: 1,
            maxLength: 200);

        ModelValidation.AddUtcDateTime(errors, lastOccurredAtUtc, nameof(lastOccurredAtUtc));
        ModelValidation.AddUtcDateTime(errors, updatedAtUtc, nameof(updatedAtUtc));

        if (updatedAtUtc < lastOccurredAtUtc)
        {
            errors.Add(ApplicationErrors.Validation("updatedAtUtc cannot be earlier than lastOccurredAtUtc."));
        }

        var normalizedLastActorUserId = ModelValidation.ValidateOptionalString(
            errors,
            lastActorUserId,
            nameof(lastActorUserId),
            maxLength: 256);

        var normalizedLastActorAnonymousId = ModelValidation.ValidateOptionalString(
            errors,
            lastActorAnonymousId,
            nameof(lastActorAnonymousId),
            maxLength: 256);

        return errors.Count > 0
            ? Result.Fail<ChainCursor>(errors)
            : Result.Ok(new ChainCursor(
                chainKey!,
                lastEventId,
                normalizedLastEventName,
                lastOccurredAtUtc,
                normalizedLastActorUserId,
                normalizedLastActorAnonymousId,
                updatedAtUtc));
    }

    public static ChainCursor Rehydrate(
        ChainKey chainKey,
        Guid lastEventId,
        string lastEventName,
        DateTime lastOccurredAtUtc,
        string? lastActorUserId,
        string? lastActorAnonymousId,
        DateTime updatedAtUtc)
        => new(
            chainKey,
            lastEventId,
            lastEventName,
            lastOccurredAtUtc,
            lastActorUserId,
            lastActorAnonymousId,
            updatedAtUtc);
}