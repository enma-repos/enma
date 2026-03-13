using Enma.BucketBuilder.JobsOrchestration.Models;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.JobsOrchestration.Contracts;

public interface IShardCheckpointRepository
{
    Task<Result<ShardCheckpoint?>> LoadAsync(PipelineName pipeline, ShardDescriptor shard, CancellationToken ct = default);

    Task<Result> SaveAsync(ShardCheckpoint checkpoint, CancellationToken ct = default);

    Task<Result> SaveAndRenewLeaseAsync(
        ShardCheckpoint checkpoint,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);

    Task<Result<LeaseToken?>> TryAcquireLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);

    Task<Result> ReleaseLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        CancellationToken ct = default);

    Task<Result> RenewLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);
}
