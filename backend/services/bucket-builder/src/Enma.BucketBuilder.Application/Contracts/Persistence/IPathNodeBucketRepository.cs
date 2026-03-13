using Enma.BucketBuilder.Application.Models;
using FluentResults;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathNodeBucketRepository
{
    Task<Result> UpsertBatchAsync(IReadOnlyCollection<PathNodeBucket> buckets, CancellationToken ct = default);
}
