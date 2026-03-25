using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class OrganizationInviteMapper
{
    internal static OrganizationInvite ToModel(this OrganizationInviteEntity entity)
        => OrganizationInvite.Rehydrate(
            id: entity.Id,
            orgId: entity.OrganizationId,
            organizationName: entity.Organization?.Name ?? "",
            email: entity.TargetEmail,
            role: entity.Role,
            createdByUserId: entity.CreatedByUserId,
            acceptedUserId: entity.AcceptedUserId,
            createdAt: entity.CreatedAt,
            expiresAt: entity.ExpiresAt,
            acceptedAt: entity.AcceptedAt,
            declinedUserId: entity.DeclinedUserId,
            declinedAt: entity.DeclinedAt);

    internal static OrganizationInviteEntity ToEntity(this OrganizationInvite model)
        => new()
        {
            Id = model.Id,
            OrganizationId = model.OrganizationId,
            TargetEmail = model.TargetEmail.Value,
            Role = model.Role,
            ExpiresAt = model.ExpiresAt,
            CreatedByUserId = model.CreatedByUserId,
            AcceptedUserId = model.AcceptedUserId,
            CreatedAt = model.CreatedAt,
            AcceptedAt = model.AcceptedAt,
            DeclinedUserId = model.DeclinedUserId,
            DeclinedAt = model.DeclinedAt
        };
}
