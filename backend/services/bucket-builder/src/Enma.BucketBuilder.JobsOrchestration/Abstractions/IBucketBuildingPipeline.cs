using Enma.BucketBuilder.JobsOrchestration.Models;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;

namespace Enma.BucketBuilder.JobsOrchestration.Abstractions;

public interface IBucketBuildingPipeline
{
    Task<ShardRunResult?> RunTickAsync(ShardDescriptor shard, CancellationToken ct = default);
}
