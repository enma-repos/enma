using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class OrganizationMemberMapper
{
    internal static OrganizationMember ToModel(this OrganizationMemberEntity entity)
        => OrganizationMember.Rehydrate(
            orgId: entity.OrganizationId,
            userId: entity.UserId,
            role: entity.Role,
            status: entity.Status,
            displayName: entity.User?.DisplayName ?? "",
            email: entity.User?.Email ?? "",
            avatarUrl: entity.User?.AvatarUrl,
            joinedAt: entity.JoinedAt,
            updatedAt: entity.UpdatedAt);

    internal static OrganizationMemberEntity ToEntity(this OrganizationMember model)
        => new()
        {
            OrganizationId = model.OrganizationId,
            UserId = model.UserId,
            Role = model.Role,
            Status = model.Status,
            JoinedAt = model.JoinedAt,
            UpdatedAt = model.UpdatedAt
        };
}
