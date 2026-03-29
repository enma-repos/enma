using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class TopEventsService(
    IPathNodeQueryRepository nodeRepo) : ITopEventsService
{
    private static readonly HashSet<string> AllowedSortFields =
        ["visits", "entries", "exits", "unique_chains"];

    public async Task<Result<TopEventsDto>> GetTopEventsAsync(
        ProcessFilter filter, string sortBy, int limit, CancellationToken ct = default)
    {
        sortBy = (sortBy ?? "visits").Trim().ToLowerInvariant();

        if (!AllowedSortFields.Contains(sortBy))
            return Result.Fail<TopEventsDto>(
                ApplicationErrors.Validation($"sortBy must be one of: {string.Join(", ", AllowedSortFields)}."));

        if (limit is < 1 or > 100)
            return Result.Fail<TopEventsDto>(
                ApplicationErrors.Validation("limit must be between 1 and 100."));

        var nodesResult = await nodeRepo.GetAggregatedNodesAsync(filter, ct);
        if (nodesResult.IsFailed)
            return Result.Fail<TopEventsDto>(nodesResult.Errors);

        Func<AggregatedNode, long> sortSelector = sortBy switch
        {
            "entries" => n => n.TotalEntries,
            "exits" => n => n.TotalExits,
            "unique_chains" => n => n.TotalUniqueChains,
            _ => n => n.TotalVisits
        };

        var events = nodesResult.Value
            .OrderByDescending(sortSelector)
            .Take(limit)
            .Select(n => new TopEventItemDto(n.EventName, n.TotalVisits, n.TotalEntries, n.TotalExits, n.TotalUniqueChains))
            .ToList();

        return Result.Ok(new TopEventsDto(events));
    }

    public async Task<Result<TopEventsDto>> GetTopEventsAsync(
        Guid organizationId, Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange, string sortBy, int limit,
        CancellationToken ct = default)
    {
        sortBy = (sortBy ?? "visits").Trim().ToLowerInvariant();

        if (!AllowedSortFields.Contains(sortBy))
            return Result.Fail<TopEventsDto>(
                ApplicationErrors.Validation($"sortBy must be one of: {string.Join(", ", AllowedSortFields)}."));

        if (limit is < 1 or > 100)
            return Result.Fail<TopEventsDto>(
                ApplicationErrors.Validation("limit must be between 1 and 100."));

        Result<IReadOnlyList<AggregatedNode>> nodesResult;

        if (processDefinitionIds is null or { Count: 0 })
        {
            var filter = new ProjectFilter(organizationId, projectId, dateRange);
            nodesResult = await nodeRepo.GetAggregatedNodesAsync(filter, ct);
        }
        else
        {
            var filter = new MultiProcessFilter(organizationId, projectId, processDefinitionIds, dateRange);
            nodesResult = await nodeRepo.GetAggregatedNodesAsync(filter, ct);
        }

        if (nodesResult.IsFailed)
            return Result.Fail<TopEventsDto>(nodesResult.Errors);

        Func<AggregatedNode, long> sortSelector = sortBy switch
        {
            "entries" => n => n.TotalEntries,
            "exits" => n => n.TotalExits,
            "unique_chains" => n => n.TotalUniqueChains,
            _ => n => n.TotalVisits
        };

        var events = nodesResult.Value
            .OrderByDescending(sortSelector)
            .Take(limit)
            .Select(n => new TopEventItemDto(n.EventName, n.TotalVisits, n.TotalEntries, n.TotalExits, n.TotalUniqueChains))
            .ToList();

        return Result.Ok(new TopEventsDto(events));
    }
}
