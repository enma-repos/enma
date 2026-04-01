using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class SummaryService(
    IPathNodeQueryRepository nodeRepo,
    IPathEdgeQueryRepository edgeRepo,
    IUniqueUsersQueryRepository uniqueUsersRepo) : ISummaryService
{
    public async Task<Result<SummaryDto>> GetSummaryAsync(
        Guid organizationId,
        Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange,
        CancellationToken ct = default)
    {
        var previousRange = ComputePreviousPeriod(dateRange);
        if (previousRange.IsFailed)
            return Result.Fail<SummaryDto>(previousRange.Errors);

        var currentNodes = QueryNodeSummary(organizationId, projectId, processDefinitionIds, dateRange, ct);
        var currentEdges = QueryEdgeSummary(organizationId, projectId, processDefinitionIds, dateRange, ct);
        var prevNodes = QueryNodeSummary(organizationId, projectId, processDefinitionIds, previousRange.Value, ct);
        var prevEdges = QueryEdgeSummary(organizationId, projectId, processDefinitionIds, previousRange.Value, ct);
        var currentUsers = uniqueUsersRepo.GetUniqueUsersAsync(organizationId, projectId, processDefinitionIds, dateRange, ct);
        var prevUsers = uniqueUsersRepo.GetUniqueUsersAsync(organizationId, projectId, processDefinitionIds, previousRange.Value, ct);

        await Task.WhenAll(currentNodes, currentEdges, prevNodes, prevEdges, currentUsers, prevUsers);

        if (currentNodes.Result.IsFailed) return Result.Fail<SummaryDto>(currentNodes.Result.Errors);
        if (currentEdges.Result.IsFailed) return Result.Fail<SummaryDto>(currentEdges.Result.Errors);
        if (prevNodes.Result.IsFailed) return Result.Fail<SummaryDto>(prevNodes.Result.Errors);
        if (prevEdges.Result.IsFailed) return Result.Fail<SummaryDto>(prevEdges.Result.Errors);
        if (currentUsers.Result.IsFailed) return Result.Fail<SummaryDto>(currentUsers.Result.Errors);
        if (prevUsers.Result.IsFailed) return Result.Fail<SummaryDto>(prevUsers.Result.Errors);

        var cn = currentNodes.Result.Value;
        var ce = currentEdges.Result.Value;
        var pn = prevNodes.Result.Value;
        var pe = prevEdges.Result.Value;
        var cu = currentUsers.Result.Value;
        var pu = prevUsers.Result.Value;

        var totalVisits = BuildMetric(cn.TotalVisits, pn.TotalVisits);
        var uniqueChains = BuildMetric(cn.TotalEntries, pn.TotalEntries);
        var uniqueUsers = BuildMetric(
            cu.Users + cu.Anonymous,
            pu.Users + pu.Anonymous);

        var currentAvg = cn.TotalEntries > 0
            ? Math.Round((double)ce.TotalTransitions / cn.TotalEntries, 1)
            : 0.0;
        var prevAvg = pn.TotalEntries > 0
            ? Math.Round((double)pe.TotalTransitions / pn.TotalEntries, 1)
            : 0.0;
        var avgSteps = BuildMetric(currentAvg, prevAvg);

        return Result.Ok(new SummaryDto(totalVisits, uniqueChains, uniqueUsers, avgSteps));
    }

    private Task<Result<NodeSummary>> QueryNodeSummary(
        Guid organizationId, Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange, CancellationToken ct)
    {
        if (processDefinitionIds is null or { Count: 0 })
        {
            var filter = new ProjectFilter(organizationId, projectId, dateRange);
            return nodeRepo.GetNodeSummaryAsync(filter, ct);
        }
        else
        {
            var filter = new MultiProcessFilter(organizationId, projectId, processDefinitionIds, dateRange);
            return nodeRepo.GetNodeSummaryAsync(filter, ct);
        }
    }

    private Task<Result<EdgeSummary>> QueryEdgeSummary(
        Guid organizationId, Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange, CancellationToken ct)
    {
        if (processDefinitionIds is null or { Count: 0 })
        {
            var filter = new ProjectFilter(organizationId, projectId, dateRange);
            return edgeRepo.GetEdgeSummaryAsync(filter, ct);
        }
        else
        {
            var filter = new MultiProcessFilter(organizationId, projectId, processDefinitionIds, dateRange);
            return edgeRepo.GetEdgeSummaryAsync(filter, ct);
        }
    }

    private static Result<DateRange> ComputePreviousPeriod(DateRange current)
    {
        var duration = current.ToUtc - current.FromUtc;
        var prevFrom = current.FromUtc - duration;
        var prevTo = current.FromUtc;
        return DateRange.Create(prevFrom, prevTo);
    }

    private static SummaryMetricDto BuildMetric(double current, double previous)
    {
        var absolute = current - previous;
        var percent = previous != 0
            ? Math.Round(absolute / previous * 100, 1)
            : current != 0 ? 100.0 : 0.0;
        return new SummaryMetricDto(current, percent, absolute);
    }
}
