namespace Enma.Analytics.Application.Dto;

public sealed record FunnelAnalysisRequest(IReadOnlyList<string> Steps);

public sealed record FunnelAnalysisDto(IReadOnlyList<FunnelStepDto> Steps);

public sealed record FunnelStepDto(
    string EventName,
    int StepIndex,
    long Visits,
    double ConversionRateFromPrevious,
    double ConversionRateFromFirst);
