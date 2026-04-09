using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperUserOrganizationMembershipDto(
    Guid OrganizationId,
    string OrganizationName,
    string OrganizationSlug,
    OrganizationRole Role,
    DateTime JoinedAt);

public sealed record SuperUserProjectMembershipDto(
    Guid ProjectId,
    string ProjectName,
    string ProjectKey,
    Guid OrganizationId,
    string OrganizationName,
    ProjectRole Role,
    DateTime JoinedAt);

public sealed record SuperUserDetailsDto(
    Guid Id,
    string Email,
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt,
    IReadOnlyList<SuperUserOrganizationMembershipDto> Organizations,
    IReadOnlyList<SuperUserProjectMembershipDto> Projects);
