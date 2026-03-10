using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Dedup key for protecting window aggregation from repeated source rows.
/// </summary>
public sealed class SourceEventDedupKey
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
        ModelValidation.AddRequiredGuid(errors, eventId, nameof(eventId));

        if (chainKey is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(chainKey)));
        }

        return errors.Count > 0
            ? Result.Fail<SourceEventDedupKey>(errors)
            : Result.Ok(new SourceEventDedupKey(eventId, chainKey!));
    }

    public static SourceEventDedupKey Rehydrate(Guid eventId, ChainKey chainKey) => new(eventId, chainKey);
}
