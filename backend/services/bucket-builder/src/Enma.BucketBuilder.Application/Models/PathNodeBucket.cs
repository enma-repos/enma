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
    public DateTime BucketEndUtc { get; }
    public long VisitsCount { get; }
    public long EntriesCount { get; }
    public long ExitsCount { get; }
    public long UniqueChains { get; }
    public int ProjectShard { get; }
    public Guid BuildId { get; }

    private PathNodeBucket(
        NodeKey key,
        DateTime bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains,
        int projectShard,
        Guid buildId)
    {
        Key = key;
        BucketEndUtc = bucketEndUtc;
        VisitsCount = visitsCount;
        EntriesCount = entriesCount;
        ExitsCount = exitsCount;
        UniqueChains = uniqueChains;
        ProjectShard = projectShard;
        BuildId = buildId;
    }

    public static Result<PathNodeBucket> Create(
        NodeKey? key,
        DateTime bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains,
        int projectShard,
        Guid buildId)
    {
        var errors = new List<IError>();

        if (key is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(key)));
        }

        ModelValidation.AddUtcDateTime(errors, bucketEndUtc, nameof(bucketEndUtc));
        if (!ModelValidation.IsFiveMinuteBoundary(bucketEndUtc))
        {
            errors.Add(ApplicationErrors.Validation("bucketEndUtc must be aligned to a 5-minute boundary."));
        }

        if (key is not null && bucketEndUtc - key.BucketStartUtc != BucketSize)
        {
            errors.Add(ApplicationErrors.Validation(
                "bucketEndUtc must be exactly 5 minutes after key.BucketStartUtc."));
        }

        ModelValidation.AddNonNegativeLong(errors, visitsCount, nameof(visitsCount));
        ModelValidation.AddNonNegativeLong(errors, entriesCount, nameof(entriesCount));
        ModelValidation.AddNonNegativeLong(errors, exitsCount, nameof(exitsCount));
        ModelValidation.AddNonNegativeLong(errors, uniqueChains, nameof(uniqueChains));
        ModelValidation.AddNonNegativeInt(errors, projectShard, nameof(projectShard));
        ModelValidation.AddRequiredGuid(errors, buildId, nameof(buildId));

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
                bucketEndUtc,
                visitsCount,
                entriesCount,
                exitsCount,
                uniqueChains,
                projectShard,
                buildId));
    }

    public static PathNodeBucket Rehydrate(
        NodeKey key,
        DateTime bucketEndUtc,
        long visitsCount,
        long entriesCount,
        long exitsCount,
        long uniqueChains,
        int projectShard,
        Guid buildId)
        => new PathNodeBucket(
            key,
            bucketEndUtc,
            visitsCount,
            entriesCount,
            exitsCount,
            uniqueChains,
            projectShard,
            buildId);
}