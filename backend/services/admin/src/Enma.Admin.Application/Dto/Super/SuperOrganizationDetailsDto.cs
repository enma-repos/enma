using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperOrganizationMemberDto(
    Guid UserId,
    string Email,
    string DisplayName,
    string? AvatarUrl,
    OrganizationRole Role,
    DateTime JoinedAt);

public sealed record SuperOrganizationProjectDto(
    Guid Id,
    string Name,
    string Key,
    int MemberCount,
    DateTime CreatedAt,
    DateTime? DeletedAt,
    DateTime? ArchivedAt);

public sealed record SuperOrganizationDetailsDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    Guid OwnerUserId,
    string? OwnerEmail,
    string? OwnerDisplayName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt,
    IReadOnlyList<SuperOrganizationMemberDto> Members,
    IReadOnlyList<SuperOrganizationProjectDto> Projects);
