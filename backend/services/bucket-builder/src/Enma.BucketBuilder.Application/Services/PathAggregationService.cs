using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Services;

internal sealed class PathAggregationService : IPathAggregationService
{
    private readonly IChainStitchingService _chainStitchingService;

    public PathAggregationService(IChainStitchingService chainStitchingService)
    {
        ArgumentNullException.ThrowIfNull(chainStitchingService);
        _chainStitchingService = chainStitchingService;
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
                errors.AddRange(sliceResult.Errors);
                continue;
            }

            slices.Add(sliceResult.Value);
        }

        if (errors.Count > 0)
        {
            return Result.Fail<WindowProjectionBatch>(errors);
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
            var visits = group.ToArray();

            var bucketResult = PathNodeBucket.Create(
                group.Key,
                window.EndUtc.Value,
                visits.LongLength,
                visits.LongCount(x => x.IsEntry),
                visits.LongCount(x => x.IsExit),
                visits.Select(x => x.ChainKey).Distinct().LongCount());

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
            var transitions = group.ToArray();

            var bucketResult = PathEdgeBucket.Create(
                group.Key,
                window.EndUtc.Value,
                transitions.LongLength,
                transitions.Select(x => x.ChainKey).Distinct().LongCount(),
                transitions.Where(x => x.ActorUserId is not null).Select(x => x.ActorUserId!).Distinct().LongCount(),
                transitions.Where(x => x.ActorAnonymousId is not null).Select(x => x.ActorAnonymousId!).Distinct().LongCount());

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
