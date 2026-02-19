using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class OrganizationInviteMapper
{
    internal static OrganizationInvite ToModel(this OrganizationInviteEntity entity)
        => OrganizationInvite.Rehydrate(
            id: entity.Id,
            orgId: entity.OrganizationId,
            email: entity.TargetEmail,
            role: entity.Role,
            tokenHash: entity.TokenHash,
            createdByUserId: entity.CreatedByUserId,
            acceptedUserId: entity.AcceptedUserId,
            createdAt: entity.CreatedAt,
            expiresAt: entity.ExpiresAt,
            acceptedAt: entity.AcceptedAt);

    internal static OrganizationInviteEntity ToEntity(this OrganizationInvite model)
        => new()
        {
            Id = model.Id,
            OrganizationId = model.OrganizationId,
            TargetEmail = model.TargetEmail.Value,
            Role = model.Role,
            TokenHash = model.TokenHash,
            ExpiresAt = model.ExpiresAt,
            CreatedByUserId = model.CreatedByUserId,
            AcceptedUserId = model.AcceptedUserId,
            CreatedAt = model.CreatedAt,
            AcceptedAt = model.AcceptedAt
        };
}

