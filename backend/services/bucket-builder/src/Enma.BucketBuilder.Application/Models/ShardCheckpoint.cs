using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Persistent processing state for one pipeline shard:
/// checkpoint, lease owner and lease TTL.
/// </summary>
public sealed class ShardCheckpoint
{
    public PipelineName Pipeline { get; }
    public ShardDescriptor Shard { get; }
    public BucketBoundaryUtc? LastCompletedBucketEndUtc { get; }
    public LeaseOwnerId? LeaseOwner { get; }
    public DateTime? LeaseUntilUtc { get; }
    public DateTime UpdatedAtUtc { get; }

    private ShardCheckpoint(
        PipelineName pipeline,
        ShardDescriptor shard,
        BucketBoundaryUtc? lastCompletedBucketEndUtc,
        LeaseOwnerId? leaseOwner,
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
        ShardDescriptor shard,
        DateTime? lastCompletedBucketEndUtc,
        string? leaseOwner,
        DateTime? leaseUntilUtc,
        DateTime updatedAtUtc)
    {
        var errors = new List<IError>();

        var pipelineVoResult = PipelineName.Create(pipeline);
        if (pipelineVoResult.IsFailed)
        {
            errors.AddRange(pipelineVoResult.Errors);
        }

        Result<BucketBoundaryUtc>? lastCompletedVoResult = null;
        if (lastCompletedBucketEndUtc.HasValue)
        {
            lastCompletedVoResult = BucketBoundaryUtc.Create(lastCompletedBucketEndUtc.Value);
            if (lastCompletedVoResult.IsFailed)
            {
                errors.AddRange(lastCompletedVoResult.Errors);
            }
        }

        var leaseOwnerVoResult = LeaseOwnerId.CreateOptional(leaseOwner);
        if (leaseOwnerVoResult.IsFailed)
        {
            errors.AddRange(leaseOwnerVoResult.Errors);
        }

        if (leaseUntilUtc.HasValue && leaseUntilUtc.Value.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(leaseUntilUtc)} must be UTC when provided."));
        }

        if (leaseOwnerVoResult.Value is null ^ leaseUntilUtc is null)
        {
            errors.Add(ApplicationErrors.Validation("leaseOwner and leaseUntilUtc must be set together or both be null."));
        }

        if (updatedAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(updatedAtUtc)} must be UTC."));
        }

        return errors.Count > 0
            ? Result.Fail<ShardCheckpoint>(errors)
            : Result.Ok(new ShardCheckpoint(
                pipelineVoResult.Value,
                shard,
                lastCompletedVoResult?.Value,
                leaseOwnerVoResult.Value,
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
        => new(
            PipelineName.Rehydrate(pipeline),
            shard,
            lastCompletedBucketEndUtc.HasValue ? BucketBoundaryUtc.Rehydrate(lastCompletedBucketEndUtc.Value) : null,
            string.IsNullOrWhiteSpace(leaseOwner) ? null : LeaseOwnerId.Rehydrate(leaseOwner),
            leaseUntilUtc,
            updatedAtUtc);
}
