using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.OrganizationInvites;

internal static class OrganizationInviteDtoMapper
{
    internal static OrganizationInviteDto ToDto(this OrganizationInvite model)
        => new(
            Id: model.Id,
            OrganizationId: model.OrganizationId,
            OrganizationName: model.OrganizationName,
            TargetEmail: model.TargetEmail.Value,
            Role: model.Role,
            ExpiresAt: model.ExpiresAt,
            CreatedByUserId: model.CreatedByUserId,
            AcceptedUserId: model.AcceptedUserId,
            CreatedAt: model.CreatedAt,
            AcceptedAt: model.AcceptedAt,
            DeclinedUserId: model.DeclinedUserId,
            DeclinedAt: model.DeclinedAt);
}
