using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class EventDetailService(
    IPathNodeQueryRepository nodeRepo,
    IPathEdgeQueryRepository edgeRepo) : IEventDetailService
{
    public async Task<Result<EventDetailDto>> GetEventDetailAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(eventName))
            return Result.Fail<EventDetailDto>(ApplicationErrors.Required("eventName"));

        var nodeTask = nodeRepo.GetSingleNodeAsync(filter, eventName, ct);
        var edgesTask = edgeRepo.GetEdgesForEventAsync(filter, eventName, ct);

        await Task.WhenAll(nodeTask, edgesTask);

        var nodeResult = await nodeTask;
        if (nodeResult.IsFailed)
            return Result.Fail<EventDetailDto>(nodeResult.Errors);

        var edgesResult = await edgesTask;
        if (edgesResult.IsFailed)
            return Result.Fail<EventDetailDto>(edgesResult.Errors);

        var node = nodeResult.Value;
        if (node is null)
            return Result.Fail<EventDetailDto>(ApplicationErrors.EntityNotFound("Event", eventName));

        var incoming = edgesResult.Value
            .Where(e => e.ToEvent == eventName && e.FromEvent != eventName)
            .Select(e => new FlowEdgeDto(e.FromEvent, e.ToEvent, e.TotalTransitions, e.TotalUniqueChains))
            .ToList();

        var outgoing = edgesResult.Value
            .Where(e => e.FromEvent == eventName && e.ToEvent != eventName)
            .Select(e => new FlowEdgeDto(e.FromEvent, e.ToEvent, e.TotalTransitions, e.TotalUniqueChains))
            .ToList();

        return Result.Ok(new EventDetailDto(
            node.EventName,
            node.TotalVisits,
            node.TotalEntries,
            node.TotalExits,
            node.TotalUniqueChains,
            incoming,
            outgoing));
    }
}
