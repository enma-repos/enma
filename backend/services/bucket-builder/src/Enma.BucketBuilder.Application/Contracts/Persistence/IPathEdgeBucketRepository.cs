using Enma.BucketBuilder.Application.Models;
using FluentResults;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathEdgeBucketRepository
{
    Task<Result> UpsertBatchAsync(IReadOnlyCollection<PathEdgeBucket> buckets, CancellationToken ct = default);
}
