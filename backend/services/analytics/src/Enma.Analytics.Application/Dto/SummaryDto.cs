namespace Enma.Analytics.Application.Dto;

public sealed record SummaryDto(
    SummaryMetricDto TotalVisits,
    SummaryMetricDto UniqueChains,
    SummaryMetricDto UniqueUsers,
    SummaryMetricDto AvgStepsPerChain);

public sealed record SummaryMetricDto(
    double Value,
    double TrendPercent,
    double TrendAbsolute);

public sealed record SummaryRequest(IReadOnlyList<Guid>? ProcessDefinitionIds);
