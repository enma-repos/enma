using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class EntryExitPointsService(
    IPathNodeQueryRepository nodeRepo) : IEntryExitPointsService
{
    public async Task<Result<EntryExitPointsDto>> GetEntryExitPointsAsync(
        ProcessFilter filter, int limit, CancellationToken ct = default)
    {
        if (limit is < 1 or > 100)
            return Result.Fail<EntryExitPointsDto>(
                ApplicationErrors.Validation("limit must be between 1 and 100."));

        var nodesResult = await nodeRepo.GetAggregatedNodesAsync(filter, ct);
        if (nodesResult.IsFailed)
            return Result.Fail<EntryExitPointsDto>(nodesResult.Errors);

        var nodes = nodesResult.Value;

        var entries = nodes
            .Where(n => n.TotalEntries > 0)
            .OrderByDescending(n => n.TotalEntries)
            .Take(limit)
            .Select(n => new EntryExitPointDto(
                n.EventName,
                n.TotalEntries,
                n.TotalVisits > 0 ? Math.Round((double)n.TotalEntries / n.TotalVisits * 100, 2) : 0.0))
            .ToList();

        var exits = nodes
            .Where(n => n.TotalExits > 0)
            .OrderByDescending(n => n.TotalExits)
            .Take(limit)
            .Select(n => new EntryExitPointDto(
                n.EventName,
                n.TotalExits,
                n.TotalVisits > 0 ? Math.Round((double)n.TotalExits / n.TotalVisits * 100, 2) : 0.0))
            .ToList();

        return Result.Ok(new EntryExitPointsDto(entries, exits));
    }
}
