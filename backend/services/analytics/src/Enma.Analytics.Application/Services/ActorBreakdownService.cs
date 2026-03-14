using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class ActorBreakdownService(
    IPathEdgeQueryRepository edgeRepo) : IActorBreakdownService
{
    public async Task<Result<ActorBreakdownDto>> GetActorBreakdownAsync(
        ProcessFilter filter, CancellationToken ct = default)
    {
        var edgesResult = await edgeRepo.GetAggregatedEdgesAsync(filter, ct);
        if (edgesResult.IsFailed)
            return Result.Fail<ActorBreakdownDto>(edgesResult.Errors);

        var items = edgesResult.Value
            .Select(e => new ActorBreakdownItemDto(
                e.FromEvent,
                e.ToEvent,
                e.TotalUniqueUsers,
                e.TotalUniqueAnonymous,
                e.TotalTransitions,
                e.TotalTransitions > 0 ? Math.Round((double)e.TotalUniqueUsers / e.TotalTransitions * 100, 2) : 0.0,
                e.TotalTransitions > 0 ? Math.Round((double)e.TotalUniqueAnonymous / e.TotalTransitions * 100, 2) : 0.0))
            .ToList();

        return Result.Ok(new ActorBreakdownDto(items));
    }
}
