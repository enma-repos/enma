namespace Enma.Analytics.Application.Dto;

public sealed record TimeTrendsDto(
    IReadOnlyList<TimeTrendPointDto> Points,
    string Granularity);

public sealed record TimeTrendPointDto(
    DateTime BucketStart,
    long Visits,
    long Entries,
    long Exits,
    long UniqueChains);
