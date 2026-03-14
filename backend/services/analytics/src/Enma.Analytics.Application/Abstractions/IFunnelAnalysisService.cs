using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IFunnelAnalysisService
{
    Task<Result<FunnelAnalysisDto>> AnalyzeAsync(
        ProcessFilter filter, FunnelAnalysisRequest request, CancellationToken ct = default);
}
