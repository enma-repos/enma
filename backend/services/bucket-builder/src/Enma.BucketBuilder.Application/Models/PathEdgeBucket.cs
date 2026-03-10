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
    public DateTime BucketEndUtc { get; }
    public long TransitionsCount { get; }
    public long UniqueChains { get; }
    public long UniqueUsers { get; }
    public long UniqueAnonymous { get; }
    public int ProjectShard { get; }
    public Guid BuildId { get; }

    private PathEdgeBucket(
        EdgeKey key,
        DateTime bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
        int projectShard,
        Guid buildId)
    {
        Key = key;
        BucketEndUtc = bucketEndUtc;
        TransitionsCount = transitionsCount;
        UniqueChains = uniqueChains;
        UniqueUsers = uniqueUsers;
        UniqueAnonymous = uniqueAnonymous;
        ProjectShard = projectShard;
        BuildId = buildId;
    }

    public static Result<PathEdgeBucket> Create(
        EdgeKey? key,
        DateTime bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
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

        ModelValidation.AddNonNegativeLong(errors, transitionsCount, nameof(transitionsCount));
        ModelValidation.AddNonNegativeLong(errors, uniqueChains, nameof(uniqueChains));
        ModelValidation.AddNonNegativeLong(errors, uniqueUsers, nameof(uniqueUsers));
        ModelValidation.AddNonNegativeLong(errors, uniqueAnonymous, nameof(uniqueAnonymous));
        ModelValidation.AddNonNegativeInt(errors, projectShard, nameof(projectShard));
        ModelValidation.AddRequiredGuid(errors, buildId, nameof(buildId));

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

        return errors.Count > 0
            ? Result.Fail<PathEdgeBucket>(errors)
            : Result.Ok(new PathEdgeBucket(
                key!,
                bucketEndUtc,
                transitionsCount,
                uniqueChains,
                uniqueUsers,
                uniqueAnonymous,
                projectShard,
                buildId));
    }

    public static PathEdgeBucket Rehydrate(
        EdgeKey key,
        DateTime bucketEndUtc,
        long transitionsCount,
        long uniqueChains,
        long uniqueUsers,
        long uniqueAnonymous,
        int projectShard,
        Guid buildId)
        => new PathEdgeBucket(
            key,
            bucketEndUtc,
            transitionsCount,
            uniqueChains,
            uniqueUsers,
            uniqueAnonymous,
            projectShard,
            buildId);
}