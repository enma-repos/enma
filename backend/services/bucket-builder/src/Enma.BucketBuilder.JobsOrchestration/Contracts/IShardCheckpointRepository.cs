using Enma.BucketBuilder.JobsOrchestration.Models;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;

namespace Enma.BucketBuilder.JobsOrchestration.Contracts;

public interface IShardCheckpointRepository
{
    Task<ShardCheckpoint?> LoadAsync(PipelineName pipeline, ShardDescriptor shard, CancellationToken ct = default);

    Task SaveAsync(ShardCheckpoint checkpoint, CancellationToken ct = default);

    Task SaveAndRenewLeaseAsync(
        ShardCheckpoint checkpoint,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);

    Task<LeaseToken?> TryAcquireLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);

    Task ReleaseLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        CancellationToken ct = default);

    Task RenewLeaseAsync(
        PipelineName pipeline,
        ShardDescriptor shard,
        LeaseOwnerId ownerId,
        TimeSpan leaseTimeout,
        CancellationToken ct = default);
}
