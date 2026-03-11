using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Dedup key for protecting window aggregation from repeated source rows.
/// </summary>
internal sealed record SourceEventDedupKey
{
    public Guid EventId { get; }
    public ChainKey ChainKey { get; }

    private SourceEventDedupKey(Guid eventId, ChainKey chainKey)
    {
        EventId = eventId;
        ChainKey = chainKey;
    }

    public static Result<SourceEventDedupKey> Create(Guid eventId, ChainKey? chainKey)
    {
        var errors = new List<IError>();

        if (eventId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(eventId)));
        }

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        return errors.Count > 0
            ? Result.Fail<SourceEventDedupKey>(errors)
            : Result.Ok(new SourceEventDedupKey(eventId, chainKey!));
    }

    public static SourceEventDedupKey Rehydrate(Guid eventId, ChainKey chainKey)
        => new(eventId, chainKey);
}
