using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Lease owner identity used to guard shard execution from concurrent workers.
/// </summary>
public sealed class LeaseToken
{
    public PipelineName Pipeline { get; }
    public ShardDescriptor Shard { get; }
    public LeaseOwnerId OwnerId { get; }
    public DateTime AcquiredAtUtc { get; }
    public DateTime ExpiresAtUtc { get; }

    private LeaseToken(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
    {
        Pipeline = pipeline;
        Shard = shard;
        OwnerId = ownerId;
        AcquiredAtUtc = acquiredAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static Result<LeaseToken> Create(
        string? pipeline,
        ShardDescriptor shard,
        string? ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
    {
        var errors = new List<IError>();

        var pipelineVoResult = PipelineName.Create(pipeline);
        if (pipelineVoResult.IsFailed)
        {
            errors.AddRange(pipelineVoResult.Errors);
        }

        var ownerIdVoResult = LeaseOwnerId.Create(ownerId);
        if (ownerIdVoResult.IsFailed)
        {
            errors.AddRange(ownerIdVoResult.Errors);
        }

        if (acquiredAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(acquiredAtUtc)} must be UTC."));
        }

        if (expiresAtUtc.Kind != DateTimeKind.Utc)
        {
            errors.Add(ApplicationErrors.Validation($"{nameof(expiresAtUtc)} must be UTC."));
        }

        if (expiresAtUtc <= acquiredAtUtc)
        {
            errors.Add(ApplicationErrors.Validation("expiresAtUtc must be greater than acquiredAtUtc."));
        }

        return errors.Count > 0
            ? Result.Fail<LeaseToken>(errors)
            : Result.Ok(new LeaseToken(
                pipelineVoResult.Value,
                shard,
                ownerIdVoResult.Value,
                acquiredAtUtc,
                expiresAtUtc));
    }

    public static LeaseToken Rehydrate(
        string pipeline,
        ShardDescriptor shard,
        string ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
        => new(
            PipelineName.Rehydrate(pipeline),
            shard,
            LeaseOwnerId.Rehydrate(ownerId),
            acquiredAtUtc,
            expiresAtUtc);
}
