using Enma.BucketBuilder.Application.Models;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathNodeBucketRepository
{
    Task UpsertBatchAsync(IReadOnlyCollection<PathNodeBucket> buckets, CancellationToken ct = default);
}
