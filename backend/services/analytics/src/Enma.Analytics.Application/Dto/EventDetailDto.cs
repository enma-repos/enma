namespace Enma.Analytics.Application.Dto;

public sealed record EventDetailDto(
    string EventName,
    long TotalVisits,
    long TotalEntries,
    long TotalExits,
    long TotalUniqueChains,
    long SelfLoopTransitions,
    IReadOnlyList<FlowEdgeDto> IncomingEdges,
    IReadOnlyList<FlowEdgeDto> OutgoingEdges);
