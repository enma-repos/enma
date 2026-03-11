using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IChainCursorRepository
{
    Task<IReadOnlyDictionary<ChainKey, ChainCursor>> GetByChainKeysAsync(
        IReadOnlyCollection<ChainKey> chainKeys,
        CancellationToken ct = default);

    Task UpsertBatchAsync(IReadOnlyCollection<ChainCursor> cursors, CancellationToken ct = default);
}
