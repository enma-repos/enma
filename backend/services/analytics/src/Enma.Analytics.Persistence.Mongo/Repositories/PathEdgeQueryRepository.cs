using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using Enma.Analytics.Persistence.Mongo.Connection;
using Enma.Analytics.Persistence.Mongo.Documents;
using FluentResults;
using MongoDB.Driver;

namespace Enma.Analytics.Persistence.Mongo.Repositories;

internal sealed class PathEdgeQueryRepository(IMongoDbContext db) : IPathEdgeQueryRepository
{
    public async Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesAsync(
        ProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(BuildDateRangeFilter(filter))
            .Group(
                d => new { d.FromEvent, d.ToEvent },
                g => new AggregatedEdge(
                    g.Key.FromEvent,
                    g.Key.ToEvent,
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueChains),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .SortByDescending(e => e.TotalTransitions)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedEdge>>(results);
    }

    public async Task<Result<IReadOnlyList<EdgeTimeSeries>>> GetEdgeTimeSeriesAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default)
    {
        var docs = await db.PathEdgeBuckets
            .Find(BuildFindFilter(filter))
            .ToListAsync(ct);

        var results = docs
            .GroupBy(d => TruncateToGranularity(d.BucketStartUtc, granularity))
            .OrderBy(g => g.Key)
            .Select(g => new EdgeTimeSeries(
                g.Key,
                g.Sum(x => x.TransitionsCount),
                g.Sum(x => x.UniqueChains),
                g.Sum(x => x.UniqueUsers),
                g.Sum(x => x.UniqueAnonymous)))
            .ToList();

        return Result.Ok<IReadOnlyList<EdgeTimeSeries>>(results);
    }

    public async Task<Result<IReadOnlyList<AggregatedEdge>>> GetEdgesForEventAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default)
    {
        var baseFilter = BuildDateRangeFilter(filter);
        var f = Builders<PathEdgeBucketDocument>.Filter;
        var eventFilter = f.Or(
            f.Eq(d => d.FromEvent, eventName),
            f.Eq(d => d.ToEvent, eventName));

        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(baseFilter & eventFilter)
            .Group(
                d => new { d.FromEvent, d.ToEvent },
                g => new AggregatedEdge(
                    g.Key.FromEvent,
                    g.Key.ToEvent,
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueChains),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .SortByDescending(e => e.TotalTransitions)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedEdge>>(results);
    }

    public async Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesByEntryEventAsync(
        ProcessFilter filter, string entryEventName, CancellationToken ct = default)
    {
        var baseFilter = BuildDateRangeFilter(filter);
        var entryFilter = Builders<PathEdgeBucketDocument>.Filter.Eq(d => d.EntryEventName, entryEventName);

        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(baseFilter & entryFilter)
            .Group(
                d => new { d.FromEvent, d.ToEvent },
                g => new AggregatedEdge(
                    g.Key.FromEvent,
                    g.Key.ToEvent,
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueChains),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .SortByDescending(e => e.TotalTransitions)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedEdge>>(results);
    }

    public async Task<Result<IReadOnlyList<AggregatedEdge>>> GetAggregatedEdgesAsync(
        MultiProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(BuildMultiProcessFilter(filter))
            .Group(
                d => new { d.FromEvent, d.ToEvent },
                g => new AggregatedEdge(
                    g.Key.FromEvent,
                    g.Key.ToEvent,
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueChains),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .SortByDescending(e => e.TotalTransitions)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedEdge>>(results);
    }

    public async Task<Result<EdgeSummary>> GetEdgeSummaryAsync(
        ProjectFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(BuildProjectFilter(filter))
            .Group(
                _ => 1,
                g => new EdgeSummary(
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .ToListAsync(ct);

        return Result.Ok(results.FirstOrDefault() ?? new EdgeSummary(0, 0, 0));
    }

    public async Task<Result<EdgeSummary>> GetEdgeSummaryAsync(
        MultiProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathEdgeBuckets
            .Aggregate()
            .Match(BuildMultiProcessFilter(filter))
            .Group(
                _ => 1,
                g => new EdgeSummary(
                    g.Sum(x => x.TransitionsCount),
                    g.Sum(x => x.UniqueUsers),
                    g.Sum(x => x.UniqueAnonymous)))
            .ToListAsync(ct);

        return Result.Ok(results.FirstOrDefault() ?? new EdgeSummary(0, 0, 0));
    }

    private static FilterDefinition<PathEdgeBucketDocument> BuildProjectFilter(ProjectFilter filter)
    {
        var f = Builders<PathEdgeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathEdgeBucketDocument> BuildMultiProcessFilter(MultiProcessFilter filter)
    {
        var f = Builders<PathEdgeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.In(d => d.ProcessDefinitionId, filter.ProcessDefinitionIds)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathEdgeBucketDocument> BuildDateRangeFilter(ProcessFilter filter)
    {
        var f = Builders<PathEdgeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.Eq(d => d.ProcessDefinitionId, filter.ProcessDefinitionId)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathEdgeBucketDocument> BuildFindFilter(ProcessFilter filter)
    {
        var f = Builders<PathEdgeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.Eq(d => d.ProcessDefinitionId, filter.ProcessDefinitionId)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static DateTime TruncateToGranularity(DateTime utc, Granularity granularity)
    {
        return granularity switch
        {
            Granularity.Hour => new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, DateTimeKind.Utc),
            Granularity.Day => new DateTime(utc.Year, utc.Month, utc.Day, 0, 0, 0, DateTimeKind.Utc),
            _ => utc
        };
    }
}
