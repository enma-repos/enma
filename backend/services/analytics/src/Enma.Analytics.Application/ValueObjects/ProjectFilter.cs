namespace Enma.Analytics.Application.ValueObjects;

public sealed record ProjectFilter(
    Guid OrganizationId,
    Guid ProjectId,
    DateRange DateRange);
