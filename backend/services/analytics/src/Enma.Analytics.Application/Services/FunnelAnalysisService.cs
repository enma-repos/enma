using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class FunnelAnalysisService(
    IPathNodeQueryRepository nodeRepo) : IFunnelAnalysisService
{
    public async Task<Result<FunnelAnalysisDto>> AnalyzeAsync(
        ProcessFilter filter, FunnelAnalysisRequest request, CancellationToken ct = default)
    {
        if (request.Steps is null || request.Steps.Count < 2)
            return Result.Fail<FunnelAnalysisDto>(
                ApplicationErrors.Validation("Funnel requires at least 2 steps."));

        if (request.Steps.Count > 20)
            return Result.Fail<FunnelAnalysisDto>(
                ApplicationErrors.Validation("Funnel supports at most 20 steps."));

        var nodesResult = await nodeRepo.GetNodesForEventsAsync(filter, request.Steps, ct);
        if (nodesResult.IsFailed)
            return Result.Fail<FunnelAnalysisDto>(nodesResult.Errors);

        var visitsByEvent = nodesResult.Value.ToDictionary(n => n.EventName, n => n.TotalVisits);

        var steps = new List<FunnelStepDto>(request.Steps.Count);
        var firstVisits = visitsByEvent.GetValueOrDefault(request.Steps[0], 0L);

        for (var i = 0; i < request.Steps.Count; i++)
        {
            var eventName = request.Steps[i];
            var visits = visitsByEvent.GetValueOrDefault(eventName, 0L);

            var previousVisits = i == 0
                ? visits
                : visitsByEvent.GetValueOrDefault(request.Steps[i - 1], 0L);

            var conversionFromPrevious = previousVisits > 0
                ? Math.Round((double)visits / previousVisits * 100, 2)
                : 0.0;

            var conversionFromFirst = firstVisits > 0
                ? Math.Round((double)visits / firstVisits * 100, 2)
                : 0.0;

            steps.Add(new FunnelStepDto(eventName, i, visits, conversionFromPrevious, conversionFromFirst));
        }

        return Result.Ok(new FunnelAnalysisDto(steps));
    }
}
