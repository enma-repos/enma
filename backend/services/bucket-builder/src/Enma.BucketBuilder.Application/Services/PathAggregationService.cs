using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Enma.BucketBuilder.Application.Services;

internal sealed class PathAggregationService : IPathAggregationService
{
    private readonly IChainStitchingService _chainStitchingService;
    private readonly ILogger<PathAggregationService> _logger;

    public PathAggregationService(
        IChainStitchingService chainStitchingService,
        ILogger<PathAggregationService> logger)
    {
        ArgumentNullException.ThrowIfNull(chainStitchingService);
        ArgumentNullException.ThrowIfNull(logger);
        _chainStitchingService = chainStitchingService;
        _logger = logger;
    }

    public Result<WindowProjectionBatch> Aggregate(
        BucketWindow? window,
        IReadOnlyCollection<PathSourceEvent>? sourceEvents,
        IReadOnlyDictionary<ChainKey, ChainCursor>? existingCursors)
    {
        var errors = new List<IError>();

        if (window is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(window)));
        }

        if (sourceEvents is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(sourceEvents)));
        }

        if (existingCursors is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(existingCursors)));
        }

        if (errors.Count > 0)
        {
            return Result.Fail<WindowProjectionBatch>(errors);
        }

        if (sourceEvents!.Count == 0)
        {
            return Result.Ok(WindowProjectionBatch.Empty(window!));
        }

        var uniqueEvents = sourceEvents
            .DistinctBy(x => SourceEventDedupKey.Rehydrate(x.EventId, x.ChainKey))
            .ToArray();

        var groupedEvents = uniqueEvents
            .GroupBy(x => x.ChainKey)
            .ToArray();

        var slices = new List<ChainAggregationSlice>(groupedEvents.Length);

        foreach (var group in groupedEvents)
        {
            var orderedEvents = group
                .OrderBy(x => x.OccurredAtUtc)
                .ThenBy(x => x.EventId)
                .ToArray();

            existingCursors!.TryGetValue(group.Key, out var existingCursor);

            var sliceResult = _chainStitchingService.BuildSlice(window!, existingCursor, orderedEvents);
            if (sliceResult.IsFailed)
            {
                _logger.LogWarning(
                    "Skipping chain {ChainKey} in window {WindowStart}: {Errors}",
                    group.Key,
                    window!.StartUtc,
                    string.Join("; ", sliceResult.Errors.Select(e => e.Message)));
                continue;
            }

            slices.Add(sliceResult.Value);
        }

        var nodeBucketsResult = BuildNodeBuckets(window!, slices);
        if (nodeBucketsResult.IsFailed)
        {
            return Result.Fail<WindowProjectionBatch>(nodeBucketsResult.Errors);
        }

        var edgeBucketsResult = BuildEdgeBuckets(window!, slices);
        if (edgeBucketsResult.IsFailed)
        {
            return Result.Fail<WindowProjectionBatch>(edgeBucketsResult.Errors);
        }

        var updatedCursors = slices
            .Where(x => x.UpdatedCursor is not null)
            .Select(x => x.UpdatedCursor!)
            .ToArray();

        return Result.Ok(new WindowProjectionBatch(
            window!,
            uniqueEvents.LongLength,
            groupedEvents.LongLength,
            nodeBucketsResult.Value,
            edgeBucketsResult.Value,
            updatedCursors));
    }

    private static Result<IReadOnlyCollection<PathNodeBucket>> BuildNodeBuckets(
        BucketWindow window,
        IReadOnlyCollection<ChainAggregationSlice> slices)
    {
        var errors = new List<IError>();
        var buckets = new List<PathNodeBucket>();

        foreach (var group in slices.SelectMany(x => x.NodeVisits).GroupBy(x => x.Key))
        {
            long totalVisits = 0;
            long entries = 0;
            long exits = 0;
            var uniqueChains = new HashSet<ChainKey>();

            foreach (var visit in group)
            {
                totalVisits++;
                if (visit.IsEntry) entries++;
                if (visit.IsExit) exits++;
                uniqueChains.Add(visit.ChainKey);
            }

            var bucketResult = PathNodeBucket.Create(
                group.Key,
                window.EndUtc.Value,
                totalVisits,
                entries,
                exits,
                uniqueChains.Count);

            if (bucketResult.IsFailed)
            {
                errors.AddRange(bucketResult.Errors);
                continue;
            }

            buckets.Add(bucketResult.Value);
        }

        return errors.Count > 0
            ? Result.Fail<IReadOnlyCollection<PathNodeBucket>>(errors)
            : Result.Ok<IReadOnlyCollection<PathNodeBucket>>(buckets);
    }

    private static Result<IReadOnlyCollection<PathEdgeBucket>> BuildEdgeBuckets(
        BucketWindow window,
        IReadOnlyCollection<ChainAggregationSlice> slices)
    {
        var errors = new List<IError>();
        var buckets = new List<PathEdgeBucket>();

        foreach (var group in slices.SelectMany(x => x.EdgeTransitions).GroupBy(x => x.Key))
        {
            long totalTransitions = 0;
            var uniqueChains = new HashSet<ChainKey>();
            var uniqueUsers = new HashSet<ActorIdentifier>();
            var uniqueAnonymous = new HashSet<ActorIdentifier>();

            foreach (var transition in group)
            {
                totalTransitions++;
                uniqueChains.Add(transition.ChainKey);
                if (transition.ActorUserId is not null) uniqueUsers.Add(transition.ActorUserId);
                if (transition.ActorAnonymousId is not null) uniqueAnonymous.Add(transition.ActorAnonymousId);
            }

            var bucketResult = PathEdgeBucket.Create(
                group.Key,
                window.EndUtc.Value,
                totalTransitions,
                uniqueChains.Count,
                uniqueUsers.Count,
                uniqueAnonymous.Count);

            if (bucketResult.IsFailed)
            {
                errors.AddRange(bucketResult.Errors);
                continue;
            }

            buckets.Add(bucketResult.Value);
        }

        return errors.Count > 0
            ? Result.Fail<IReadOnlyCollection<PathEdgeBucket>>(errors)
            : Result.Ok<IReadOnlyCollection<PathEdgeBucket>>(buckets);
    }
}
