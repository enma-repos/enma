using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.OrganizationMembers;

internal static class OrganizationMemberDtoMapper
{
    internal static OrganizationMemberDto ToDto(this OrganizationMember model)
        => new(
            OrganizationId: model.OrganizationId,
            UserId: model.UserId,
            Role: model.Role,
            Status: model.Status,
            JoinedAt: model.JoinedAt,
            UpdatedAt: model.UpdatedAt);
}

