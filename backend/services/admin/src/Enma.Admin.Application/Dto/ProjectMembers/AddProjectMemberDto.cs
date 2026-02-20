using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.ProjectMembers;

public sealed record AddProjectMemberDto(
    Guid ProjectId,
    Guid UserId,
    ProjectRole Role);

