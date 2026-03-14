namespace Enma.Analytics.Application.Dto;

public sealed record EntryExitPointsDto(
    IReadOnlyList<EntryExitPointDto> Entries,
    IReadOnlyList<EntryExitPointDto> Exits);

public sealed record EntryExitPointDto(
    string EventName,
    long Count,
    double Rate);
