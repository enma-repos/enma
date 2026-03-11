using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Aggregated node metrics for a 5-minute bucket.
/// </summary>
public sealed class PathNodeBucket
{
    private static readonly TimeSpan BucketSize = TimeSpan.FromMinutes(5);

    public NodeKey Key { get; }
    public BucketBoundaryUtc BucketEndUtc { get; }
    public long VisitsCount { get; }
    public long EntriesCount { get; }
    public long ExitsCount { get; }
    public long UniqueChains { get; }

    private PathNodeBucket(
        NodeKey key,
        BucketBoundaryUtc bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains)
    {
        Key = key;
        BucketEndUtc = bucketEndUtc;
        VisitsCount = visitsCount;
        EntriesCount = entriesCount;
        ExitsCount = exitsCount;
        UniqueChains = uniqueChains;
    }

    public static Result<PathNodeBucket> Create(
        NodeKey? key,
        DateTime bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains)
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

        if (visitsCount < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(visitsCount)} must be non-negative."));
        }

        if (entriesCount < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(entriesCount)} must be non-negative."));
        }

        if (exitsCount < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(exitsCount)} must be non-negative."));
        }

        if (uniqueChains < 0)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(uniqueChains)} must be non-negative."));
        }

        if (entriesCount > visitsCount)
        {
            errors.Add(ApplicationErrors.Validation("entriesCount cannot be greater than visitsCount."));
        }

        if (exitsCount > visitsCount)
        {
            errors.Add(ApplicationErrors.Validation("exitsCount cannot be greater than visitsCount."));
        }

        if (uniqueChains > visitsCount)
        {
            errors.Add(ApplicationErrors.Validation("uniqueChains cannot be greater than visitsCount."));
        }

        return errors.Count > 0
            ? Result.Fail<PathNodeBucket>(errors)
            : Result.Ok(new PathNodeBucket(
                key!,
                bucketEndVoResult.Value,
                visitsCount,
                entriesCount,
                exitsCount,
                uniqueChains));
    }

    public static PathNodeBucket Rehydrate(
        NodeKey key,
        DateTime bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains)
        => new(
            key,
            BucketBoundaryUtc.Rehydrate(bucketEndUtc),
            visitsCount,
            entriesCount,
            exitsCount,
            uniqueChains);
}
