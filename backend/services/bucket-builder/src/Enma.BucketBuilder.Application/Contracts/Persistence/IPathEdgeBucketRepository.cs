using Enma.BucketBuilder.Application.Models;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathEdgeBucketRepository
{
    Task UpsertBatchAsync(IReadOnlyCollection<PathEdgeBucket> buckets, CancellationToken ct = default);
}
