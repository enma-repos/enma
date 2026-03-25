using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.ProjectMembers;

public sealed record ProjectMemberDto(
    Guid ProjectId,
    Guid UserId,
    ProjectRole Role,
    string DisplayName,
    string Email,
    string? AvatarUrl,
    DateTime JoinedAt,
    DateTime UpdatedAt);
