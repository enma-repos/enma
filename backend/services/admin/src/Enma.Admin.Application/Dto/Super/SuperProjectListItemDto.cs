namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperProjectListItemDto(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    Guid OrganizationId,
    string OrganizationName,
    string OrganizationSlug,
    int MemberCount,
    DateTime CreatedAt,
    DateTime? DeletedAt,
    DateTime? ArchivedAt);
