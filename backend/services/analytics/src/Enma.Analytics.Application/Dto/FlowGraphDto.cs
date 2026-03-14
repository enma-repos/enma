namespace Enma.Analytics.Application.Dto;

public sealed record FlowGraphDto(
    IReadOnlyList<FlowNodeDto> Nodes,
    IReadOnlyList<FlowEdgeDto> Edges);

public sealed record FlowNodeDto(
    string EventName,
    long Visits,
    long Entries,
    long Exits,
    long UniqueChains);

public sealed record FlowEdgeDto(
    string FromEvent,
    string ToEvent,
    long Transitions,
    long UniqueChains);
