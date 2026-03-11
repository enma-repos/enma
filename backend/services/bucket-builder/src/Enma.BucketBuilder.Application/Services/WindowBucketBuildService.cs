using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Services;

internal sealed class WindowBucketBuildService : IWindowBucketBuildService
{
    private readonly IPathSourceEventReader _pathSourceEventReader;
    private readonly IPathNodeBucketRepository _pathNodeBucketRepository;
    private readonly IPathEdgeBucketRepository _pathEdgeBucketRepository;
    private readonly IChainCursorRepository _chainCursorRepository;
    private readonly IPathAggregationService _pathAggregationService;

    public WindowBucketBuildService(
        IPathSourceEventReader pathSourceEventReader,
        IPathNodeBucketRepository pathNodeBucketRepository,
        IPathEdgeBucketRepository pathEdgeBucketRepository,
        IChainCursorRepository chainCursorRepository,
        IPathAggregationService pathAggregationService)
    {
        ArgumentNullException.ThrowIfNull(pathSourceEventReader);
        ArgumentNullException.ThrowIfNull(pathNodeBucketRepository);
        ArgumentNullException.ThrowIfNull(pathEdgeBucketRepository);
        ArgumentNullException.ThrowIfNull(chainCursorRepository);
        ArgumentNullException.ThrowIfNull(pathAggregationService);

        _pathSourceEventReader = pathSourceEventReader;
        _pathNodeBucketRepository = pathNodeBucketRepository;
        _pathEdgeBucketRepository = pathEdgeBucketRepository;
        _chainCursorRepository = chainCursorRepository;
        _pathAggregationService = pathAggregationService;
    }

    public async Task<Result<WindowProjectionBatch>> BuildWindowAsync(BucketWindow? window, CancellationToken ct = default)
    {
        if (window is null)
        {
            return Result.Fail<WindowProjectionBatch>(ApplicationErrors.Required(nameof(window)));
        }

        var sourceEvents = await _pathSourceEventReader.GetWindowAsync(window, ct);
        var chainKeys = sourceEvents
            .Select(x => x.ChainKey)
            .Distinct()
            .ToArray();

        var existingCursors = chainKeys.Length == 0
            ? new Dictionary<ChainKey, ChainCursor>()
            : await _chainCursorRepository.GetByChainKeysAsync(chainKeys, ct);

        var aggregationResult = _pathAggregationService.Aggregate(window, sourceEvents, existingCursors);
        if (aggregationResult.IsFailed)
        {
            return Result.Fail<WindowProjectionBatch>(aggregationResult.Errors);
        }

        var batch = aggregationResult.Value;

        if (batch.NodeBuckets.Count > 0)
        {
            await _pathNodeBucketRepository.UpsertBatchAsync(batch.NodeBuckets, ct);
        }

        if (batch.EdgeBuckets.Count > 0)
        {
            await _pathEdgeBucketRepository.UpsertBatchAsync(batch.EdgeBuckets, ct);
        }

        if (batch.UpdatedCursors.Count > 0)
        {
            await _chainCursorRepository.UpsertBatchAsync(batch.UpdatedCursors, ct);
        }

        return Result.Ok(batch);
    }
}
