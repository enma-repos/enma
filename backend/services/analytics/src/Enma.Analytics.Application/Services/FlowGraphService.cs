using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class FlowGraphService(
    IPathNodeQueryRepository nodeRepo,
    IPathEdgeQueryRepository edgeRepo) : IFlowGraphService
{
    public async Task<Result<FlowGraphDto>> GetFlowGraphAsync(
        ProcessFilter filter, CancellationToken ct = default)
    {
        var nodesTask = nodeRepo.GetAggregatedNodesAsync(filter, ct);
        var edgesTask = edgeRepo.GetAggregatedEdgesAsync(filter, ct);

        await Task.WhenAll(nodesTask, edgesTask);

        var nodesResult = await nodesTask;
        if (nodesResult.IsFailed)
            return Result.Fail<FlowGraphDto>(nodesResult.Errors);

        var edgesResult = await edgesTask;
        if (edgesResult.IsFailed)
            return Result.Fail<FlowGraphDto>(edgesResult.Errors);

        var flowNodes = nodesResult.Value
            .Select(n => new FlowNodeDto(n.EventName, n.TotalVisits, n.TotalEntries, n.TotalExits, n.TotalUniqueChains))
            .ToList();

        var flowEdges = edgesResult.Value
            .Select(e => new FlowEdgeDto(e.FromEvent, e.ToEvent, e.TotalTransitions, e.TotalUniqueChains))
            .ToList();

        return Result.Ok(new FlowGraphDto(flowNodes, flowEdges));
    }
}
