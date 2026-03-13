using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IChainCursorRepository
{
    Task<Result<IReadOnlyDictionary<ChainKey, ChainCursor>>> GetByChainKeysAsync(
        IReadOnlyCollection<ChainKey> chainKeys,
        CancellationToken ct = default);

    Task<Result> UpsertBatchAsync(IReadOnlyCollection<ChainCursor> cursors, CancellationToken ct = default);
}
