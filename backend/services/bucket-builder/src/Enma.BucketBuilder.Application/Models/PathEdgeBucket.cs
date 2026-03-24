using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Aggregated transition metrics for a 5-minute bucket.
/// </summary>
public sealed class PathEdgeBucket
{
    private static readonly TimeSpan BucketSize = TimeSpan.FromMinutes(5);

    public EdgeKey Key { get; }
    public BucketBoundaryUtc BucketEndUtc { get; }
    public long TransitionsCount { get; }
    public long UniqueChains { get; }
    public long UniqueUsers { get; }
    public long UniqueAnonymous { get; }
    public EventName EntryEventName { get; }

    private PathEdgeBucket(
        EdgeKey key,
        BucketBoundaryUtc bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
        EventName entryEventName)
    {
        Key = key;
        BucketEndUtc = bucketEndUtc;
        TransitionsCount = transitionsCount;
        UniqueChains = uniqueChains;
        UniqueUsers = uniqueUsers;
        UniqueAnonymous = uniqueAnonymous;
        EntryEventName = entryEventName;
    }

    public static Result<PathEdgeBucket> Create(
        EdgeKey? key,
        DateTime bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
        EventName? entryEventName)
    {
        var errors = new List<IError>();

        if (key is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(key)));
        }

        var bucketEndVoResult = BucketBoundaryUtc.Create(bucketEndUtc);
        if (bucketEndVoResult.IsFailed)
        {
            errors.AddRange(bucketEndVoResult.Errors);
        }

        if (key is not null && bucketEndUtc - key.BucketStartUtc.Value != BucketSize)
        {
            errors.Add(ApplicationErrors.Validation(
                "bucketEndUtc must be exactly 5 minutes after key.BucketStartUtc."));
        }

        if (transitionsCount < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(transitionsCount)} must be non-negative."));
        }

        if (uniqueChains < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(uniqueChains)} must be non-negative."));
        }

        if (uniqueUsers < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(uniqueUsers)} must be non-negative."));
        }

        if (uniqueAnonymous < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(uniqueAnonymous)} must be non-negative."));
        }

        if (transitionsCount == 0 && (uniqueChains > 0 || uniqueUsers > 0 || uniqueAnonymous > 0))
        {
            errors.Add(ApplicationErrors.Validation("Unique counters must be zero when transitionsCount is zero."));
        }

        if (transitionsCount > 0)
        {
            if (uniqueChains > transitionsCount)
            {
                errors.Add(ApplicationErrors.Validation("uniqueChains cannot be greater than transitionsCount."));
            }

            if (uniqueUsers > transitionsCount)
            {
                errors.Add(ApplicationErrors.Validation("uniqueUsers cannot be greater than transitionsCount."));
            }

            if (uniqueAnonymous > transitionsCount)
            {
                errors.Add(ApplicationErrors.Validation("uniqueAnonymous cannot be greater than transitionsCount."));
            }
        }

        if (entryEventName is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(entryEventName)));
        }

        return errors.Count > 0
            ? Result.Fail<PathEdgeBucket>(errors)
            : Result.Ok(new PathEdgeBucket(
                key!,
                bucketEndVoResult.Value,
                transitionsCount,
                uniqueChains,
                uniqueUsers,
                uniqueAnonymous,
                entryEventName!));
    }

    public static PathEdgeBucket Rehydrate(
        EdgeKey key,
        DateTime bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
        string entryEventName)
        => new(
            key,
            BucketBoundaryUtc.Rehydrate(bucketEndUtc),
            transitionsCount,
            uniqueChains,
            uniqueUsers,
            uniqueAnonymous,
            EventName.Rehydrate(entryEventName));
}
