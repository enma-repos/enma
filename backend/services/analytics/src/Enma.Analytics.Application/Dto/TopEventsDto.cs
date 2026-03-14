namespace Enma.Analytics.Application.Dto;

public sealed record TopEventsDto(IReadOnlyList<TopEventItemDto> Events);

public sealed record TopEventItemDto(
    string EventName,
    long Visits,
    long Entries,
    long Exits,
    long UniqueChains);
