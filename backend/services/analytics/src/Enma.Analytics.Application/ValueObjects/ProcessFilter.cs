namespace Enma.Analytics.Application.ValueObjects;

public sealed record ProcessFilter(
    Guid OrganizationId,
    Guid ProjectId,
    Guid ProcessDefinitionId,
    DateRange DateRange);

public sealed record MultiProcessFilter(
    Guid OrganizationId,
    Guid ProjectId,
    IReadOnlyList<Guid> ProcessDefinitionIds,
    DateRange DateRange);
