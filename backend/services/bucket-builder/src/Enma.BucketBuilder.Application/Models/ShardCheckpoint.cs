using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Persistent processing state for one pipeline shard:
/// checkpoint, lease owner and lease TTL.
/// </summary>
public sealed class ShardCheckpoint
{
    public string Pipeline { get; }
    public ShardDescriptor Shard { get; }
    public DateTime? LastCompletedBucketEndUtc { get; }
    public string? LeaseOwner { get; }
    public DateTime? LeaseUntilUtc { get; }
    public DateTime UpdatedAtUtc { get; }

    private ShardCheckpoint(
        string pipeline,
        ShardDescriptor shard,
        DateTime? lastCompletedBucketEndUtc,
        string? leaseOwner,
        DateTime? leaseUntilUtc,
        DateTime updatedAtUtc)
    {
        Pipeline = pipeline;
        Shard = shard;
        LastCompletedBucketEndUtc = lastCompletedBucketEndUtc;
        LeaseOwner = leaseOwner;
        LeaseUntilUtc = leaseUntilUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static Result<ShardCheckpoint> Create(
        string? pipeline,
        ShardDescriptor? shard,
        DateTime? lastCompletedBucketEndUtc,
        string? leaseOwner,
        DateTime? leaseUntilUtc,
        DateTime updatedAtUtc)
    {
        var errors = new List<IError>();

        var normalizedPipeline = ModelValidation.ValidateRequiredString(
            errors,
            pipeline,
            nameof(pipeline),
            minLength: 1,
            maxLength: 64);

        if (shard is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(shard)));
        }

        if (lastCompletedBucketEndUtc.HasValue)
        {
            ModelValidation.AddUtcDateTime(errors, lastCompletedBucketEndUtc.Value, nameof(lastCompletedBucketEndUtc));
            if (!ModelValidation.IsFiveMinuteBoundary(lastCompletedBucketEndUtc.Value))
            {
                errors.Add(ApplicationErrors.Validation(
                    "lastCompletedBucketEndUtc must be aligned to a 5-minute boundary."));
            }
        }

        var normalizedLeaseOwner = ModelValidation.ValidateOptionalString(
            errors,
            leaseOwner,
            nameof(leaseOwner),
            maxLength: 200);

        if (leaseUntilUtc.HasValue)
        {
            ModelValidation.AddUtcDateTime(errors, leaseUntilUtc.Value, nameof(leaseUntilUtc));
        }

        if (normalizedLeaseOwner is null ^ leaseUntilUtc is null)
        {
            errors.Add(ApplicationErrors.Validation(
                "leaseOwner and leaseUntilUtc must be set together or both be null."));
        }

        ModelValidation.AddUtcDateTime(errors, updatedAtUtc, nameof(updatedAtUtc));

        return errors.Count > 0
            ? Result.Fail<ShardCheckpoint>(errors)
            : Result.Ok(new ShardCheckpoint(
                normalizedPipeline,
                shard!,
                lastCompletedBucketEndUtc,
                normalizedLeaseOwner,
                leaseUntilUtc,
                updatedAtUtc));
    }

    public static ShardCheckpoint Rehydrate(
        string pipeline,
        ShardDescriptor shard,
        DateTime? lastCompletedBucketEndUtc,
        string? leaseOwner,
        DateTime? leaseUntilUtc,
        DateTime updatedAtUtc)
        => new ShardCheckpoint(
            pipeline,
            shard,
            lastCompletedBucketEndUtc,
            leaseOwner,
            leaseUntilUtc,
            updatedAtUtc);
}