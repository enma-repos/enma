namespace Enma.Analytics.Application.Dto;

public sealed record ActorBreakdownDto(IReadOnlyList<ActorBreakdownItemDto> Edges);

public sealed record ActorBreakdownItemDto(
    string FromEvent,
    string ToEvent,
    long UniqueUsers,
    long UniqueAnonymous,
    long TotalTransitions,
    double UserRate,
    double AnonymousRate);
