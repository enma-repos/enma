using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.ProjectMembers;

internal static class ProjectMemberDtoMapper
{
    internal static ProjectMemberDto ToDto(this ProjectMember model)
        => new(
            ProjectId: model.ProjectId,
            UserId: model.UserId,
            Role: model.Role,
            DisplayName: model.DisplayName,
            Email: model.Email,
            AvatarUrl: model.AvatarUrl,
            JoinedAt: model.JoinedAt,
            UpdatedAt: model.UpdatedAt);
}
