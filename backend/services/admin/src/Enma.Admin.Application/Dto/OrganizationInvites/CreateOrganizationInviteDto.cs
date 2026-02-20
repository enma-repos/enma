using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.OrganizationInvites;

public sealed record CreateOrganizationInviteDto(
    Guid OrganizationId,
    string TargetEmail,
    OrganizationRole Role,
    Guid CreatedByUserId);

