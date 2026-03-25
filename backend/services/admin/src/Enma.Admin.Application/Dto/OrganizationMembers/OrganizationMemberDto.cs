using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.OrganizationMembers;

public sealed record OrganizationMemberDto(
    Guid OrganizationId,
    Guid UserId,
    OrganizationRole Role,
    OrganizationMemberStatus Status,
    string DisplayName,
    string Email,
    string? AvatarUrl,
    DateTime JoinedAt,
    DateTime UpdatedAt);
