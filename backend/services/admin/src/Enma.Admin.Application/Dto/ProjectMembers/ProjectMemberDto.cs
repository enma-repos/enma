using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.ProjectMembers;

public sealed record ProjectMemberDto(
    Guid ProjectId,
    Guid UserId,
    ProjectRole Role,
    DateTime JoinedAt,
    DateTime UpdatedAt);

