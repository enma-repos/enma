using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Dedup key for protecting window aggregation from repeated source rows.
/// </summary>
public readonly record struct SourceEventDedupKey(Guid EventId, ChainKey ChainKey)
{
    public static Result<SourceEventDedupKey> Create(Guid eventId, ChainKey chainKey)
    {
        var errors = new List<IError>();

        if (eventId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(eventId)));
        }

        return errors.Count > 0
            ? Result.Fail<SourceEventDedupKey>(errors)
            : Result.Ok(new SourceEventDedupKey(eventId, chainKey));
    }

    public static SourceEventDedupKey Rehydrate(Guid eventId, ChainKey chainKey)
        => new(eventId, chainKey);
}
