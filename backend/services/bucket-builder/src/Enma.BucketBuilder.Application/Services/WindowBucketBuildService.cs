using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Contracts.Persistence;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Enma.BucketBuilder.Application.Services;

internal sealed class WindowBucketBuildService : IWindowBucketBuildService
{
    private readonly IPathSourceEventReader _pathSourceEventReader;
    private readonly IPathNodeBucketRepository _pathNodeBucketRepository;
    private readonly IPathEdgeBucketRepository _pathEdgeBucketRepository;
    private readonly IChainCursorRepository _chainCursorRepository;
    private readonly IPathAggregationService _pathAggregationService;
    private readonly ILogger<WindowBucketBuildService> _logger;

    public WindowBucketBuildService(
        IPathSourceEventReader pathSourceEventReader,
        IPathNodeBucketRepository pathNodeBucketRepository,
        IPathEdgeBucketRepository pathEdgeBucketRepository,
        IChainCursorRepository chainCursorRepository,
        IPathAggregationService pathAggregationService,
        ILogger<WindowBucketBuildService> logger)
    {
        ArgumentNullException.ThrowIfNull(pathSourceEventReader);
        ArgumentNullException.ThrowIfNull(pathNodeBucketRepository);
        ArgumentNullException.ThrowIfNull(pathEdgeBucketRepository);
        ArgumentNullException.ThrowIfNull(chainCursorRepository);
        ArgumentNullException.ThrowIfNull(pathAggregationService);
        ArgumentNullException.ThrowIfNull(logger);

        _pathSourceEventReader = pathSourceEventReader;
        _pathNodeBucketRepository = pathNodeBucketRepository;
        _pathEdgeBucketRepository = pathEdgeBucketRepository;
        _chainCursorRepository = chainCursorRepository;
        _pathAggregationService = pathAggregationService;
        _logger = logger;
    }

    public async Task<Result<WindowProjectionBatch>> BuildWindowAsync(BucketWindow? window, CancellationToken ct = default)
    {
        if (window is null)
        {
            return Result.Fail<WindowProjectionBatch>(ApplicationErrors.Required(nameof(window)));
        }

        _logger.LogDebug("Processing window {WindowStart} - {WindowEnd}", window.StartUtc, window.EndUtc);

        var sourceEventsResult = await _pathSourceEventReader.GetWindowAsync(window, ct);
        if (sourceEventsResult.IsFailed)
        {
            return Result.Fail<WindowProjectionBatch>(sourceEventsResult.Errors);
        }

        var sourceEvents = sourceEventsResult.Value;

        _logger.LogDebug("Read {EventCount} source events for window {WindowStart}", sourceEvents.Count, window.StartUtc);

        var chainKeys = sourceEvents
            .Select(x => x.ChainKey)
            .Distinct()
            .ToArray();

        IReadOnlyDictionary<ChainKey, ChainCursor> existingCursors;

        if (chainKeys.Length == 0)
        {
            existingCursors = new Dictionary<ChainKey, ChainCursor>();
        }
        else
        {
            var cursorsResult = await _chainCursorRepository.GetByChainKeysAsync(chainKeys, ct);
            if (cursorsResult.IsFailed)
            {
                return Result.Fail<WindowProjectionBatch>(cursorsResult.Errors);
            }

            existingCursors = cursorsResult.Value;
        }

        var aggregationResult = _pathAggregationService.Aggregate(window, sourceEvents, existingCursors);
        if (aggregationResult.IsFailed)
        {
            return Result.Fail<WindowProjectionBatch>(aggregationResult.Errors);
        }

        var batch = aggregationResult.Value;

        if (batch.NodeBuckets.Count > 0)
        {
            var nodesResult = await _pathNodeBucketRepository.UpsertBatchAsync(batch.NodeBuckets, ct);
            if (nodesResult.IsFailed)
            {
                return Result.Fail<WindowProjectionBatch>(nodesResult.Errors);
            }
        }

        if (batch.EdgeBuckets.Count > 0)
        {
            var edgesResult = await _pathEdgeBucketRepository.UpsertBatchAsync(batch.EdgeBuckets, ct);
            if (edgesResult.IsFailed)
            {
                return Result.Fail<WindowProjectionBatch>(edgesResult.Errors);
            }
        }

        if (batch.UpdatedCursors.Count > 0)
        {
            var cursorsUpsertResult = await _chainCursorRepository.UpsertBatchAsync(batch.UpdatedCursors, ct);
            if (cursorsUpsertResult.IsFailed)
            {
                return Result.Fail<WindowProjectionBatch>(cursorsUpsertResult.Errors);
            }
        }

        _logger.LogInformation(
            "Window {WindowStart}: {Events} events, {Nodes} nodes, {Edges} edges, {Cursors} cursors",
            window.StartUtc,
            batch.EventsRead,
            batch.NodeBuckets.Count,
            batch.EdgeBuckets.Count,
            batch.UpdatedCursors.Count);

        return Result.Ok(batch);
    }
}
