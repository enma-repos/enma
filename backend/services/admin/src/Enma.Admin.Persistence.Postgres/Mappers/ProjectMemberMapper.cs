using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class ProjectMemberMapper
{
    internal static ProjectMember ToModel(this ProjectMemberEntity entity)
        => ProjectMember.Rehydrate(
            projectId: entity.ProjectId,
            userId: entity.UserId,
            role: entity.Role,
            displayName: entity.User?.DisplayName ?? "",
            email: entity.User?.Email ?? "",
            avatarUrl: entity.User?.AvatarUrl,
            joinedAt: entity.JoinedAt,
            updatedAt: entity.UpdatedAt);

    internal static ProjectMemberEntity ToEntity(this ProjectMember model)
        => new()
        {
            ProjectId = model.ProjectId,
            UserId = model.UserId,
            Role = model.Role,
            JoinedAt = model.JoinedAt,
            UpdatedAt = model.UpdatedAt
        };
}
