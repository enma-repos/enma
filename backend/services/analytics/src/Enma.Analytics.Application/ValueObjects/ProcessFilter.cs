namespace Enma.Analytics.Application.ValueObjects;

public sealed record ProcessFilter(
    Guid OrganizationId,
    Guid ProjectId,
    Guid ProcessDefinitionId,
    DateRange DateRange);
