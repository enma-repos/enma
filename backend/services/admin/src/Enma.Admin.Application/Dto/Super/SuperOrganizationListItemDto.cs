namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperOrganizationListItemDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    Guid OwnerUserId,
    string? OwnerEmail,
    string? OwnerDisplayName,
    int MemberCount,
    int ProjectCount,
    DateTime CreatedAt,
    DateTime? DeletedAt);
