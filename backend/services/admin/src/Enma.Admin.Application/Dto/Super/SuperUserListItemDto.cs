namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperUserListItemDto(
    Guid Id,
    string Email,
    string DisplayName,
    string? AvatarUrl,
    int OrganizationCount,
    int ProjectCount,
    DateTime CreatedAt,
    DateTime? DeletedAt);
