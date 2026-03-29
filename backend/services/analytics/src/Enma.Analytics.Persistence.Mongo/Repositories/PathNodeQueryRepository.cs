using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using Enma.Analytics.Persistence.Mongo.Connection;
using Enma.Analytics.Persistence.Mongo.Documents;
using FluentResults;
using MongoDB.Driver;

namespace Enma.Analytics.Persistence.Mongo.Repositories;

internal sealed class PathNodeQueryRepository(IMongoDbContext db) : IPathNodeQueryRepository
{
    public async Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesAsync(
        ProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(BuildDateRangeFilter(filter))
            .Group(
                d => d.EventName,
                g => new AggregatedNode(
                    g.Key,
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .SortByDescending(n => n.TotalVisits)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedNode>>(results);
    }

    public async Task<Result<IReadOnlyList<NodeTimeSeries>>> GetNodeTimeSeriesAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default)
    {
        // Fetch raw 5-min buckets matching the filter, then group in-memory by granularity.
        // This avoids MongoDB.Driver 3.x BsonDocument pipeline API incompatibilities
        // while keeping the $match server-side for efficiency.
        var docs = await db.PathNodeBuckets
            .Find(BuildFindFilter(filter))
            .ToListAsync(ct);

        var results = docs
            .GroupBy(d => TruncateToGranularity(d.BucketStartUtc, granularity))
            .OrderBy(g => g.Key)
            .Select(g => new NodeTimeSeries(
                g.Key,
                g.Sum(x => x.VisitsCount),
                g.Sum(x => x.EntriesCount),
                g.Sum(x => x.ExitsCount),
                g.Sum(x => x.UniqueChains)))
            .ToList();

        return Result.Ok<IReadOnlyList<NodeTimeSeries>>(results);
    }

    public async Task<Result<AggregatedNode?>> GetSingleNodeAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default)
    {
        var baseFilter = BuildDateRangeFilter(filter);
        var eventFilter = Builders<PathNodeBucketDocument>.Filter.Eq(d => d.EventName, eventName);

        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(baseFilter & eventFilter)
            .Group(
                d => d.EventName,
                g => new AggregatedNode(
                    g.Key,
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .ToListAsync(ct);

        return Result.Ok(results.FirstOrDefault());
    }

    public async Task<Result<IReadOnlyList<AggregatedNode>>> GetNodesForEventsAsync(
        ProcessFilter filter, IReadOnlyList<string> eventNames, CancellationToken ct = default)
    {
        var baseFilter = BuildDateRangeFilter(filter);
        var eventFilter = Builders<PathNodeBucketDocument>.Filter.In(d => d.EventName, eventNames);

        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(baseFilter & eventFilter)
            .Group(
                d => d.EventName,
                g => new AggregatedNode(
                    g.Key,
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedNode>>(results);
    }

    public async Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesByEntryEventAsync(
        ProcessFilter filter, string entryEventName, CancellationToken ct = default)
    {
        var baseFilter = BuildDateRangeFilter(filter);
        var entryFilter = Builders<PathNodeBucketDocument>.Filter.Eq(d => d.EntryEventName, entryEventName);

        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(baseFilter & entryFilter)
            .Group(
                d => d.EventName,
                g => new AggregatedNode(
                    g.Key,
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .SortByDescending(n => n.TotalVisits)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedNode>>(results);
    }

    public async Task<Result<IReadOnlyList<AggregatedNode>>> GetAggregatedNodesAsync(
        MultiProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(BuildMultiProcessFilter(filter))
            .Group(
                d => d.EventName,
                g => new AggregatedNode(
                    g.Key,
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .SortByDescending(n => n.TotalVisits)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AggregatedNode>>(results);
    }

    public async Task<Result<NodeSummary>> GetNodeSummaryAsync(
        ProjectFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(BuildProjectFilter(filter))
            .Group(
                _ => 1,
                g => new NodeSummary(
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .ToListAsync(ct);

        return Result.Ok(results.FirstOrDefault() ?? new NodeSummary(0, 0, 0, 0));
    }

    public async Task<Result<NodeSummary>> GetNodeSummaryAsync(
        MultiProcessFilter filter, CancellationToken ct = default)
    {
        var results = await db.PathNodeBuckets
            .Aggregate()
            .Match(BuildMultiProcessFilter(filter))
            .Group(
                _ => 1,
                g => new NodeSummary(
                    g.Sum(x => x.VisitsCount),
                    g.Sum(x => x.EntriesCount),
                    g.Sum(x => x.ExitsCount),
                    g.Sum(x => x.UniqueChains)))
            .ToListAsync(ct);

        return Result.Ok(results.FirstOrDefault() ?? new NodeSummary(0, 0, 0, 0));
    }

    private static FilterDefinition<PathNodeBucketDocument> BuildProjectFilter(ProjectFilter filter)
    {
        var f = Builders<PathNodeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathNodeBucketDocument> BuildMultiProcessFilter(MultiProcessFilter filter)
    {
        var f = Builders<PathNodeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.In(d => d.ProcessDefinitionId, filter.ProcessDefinitionIds)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathNodeBucketDocument> BuildDateRangeFilter(ProcessFilter filter)
    {
        var f = Builders<PathNodeBucketDocument>.Filter;
        return f.Eq(d => d.OrganizationId, filter.OrganizationId)
             & f.Eq(d => d.ProjectId, filter.ProjectId)
             & f.Eq(d => d.ProcessDefinitionId, filter.ProcessDefinitionId)
             & f.Gte(d => d.BucketStartUtc, filter.DateRange.FromUtc)
             & f.Lt(d => d.BucketStartUtc, filter.DateRange.ToUtc);
    }

    private static FilterDefinition<PathNodeBucketDocument> BuildFindFilter(ProcessFilter filter)
    {
        var f = Builders<PathNodeBucketDocument>.Filter;
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
            _ => utc // FiveMinutes — already aligned
        };
    }
}
