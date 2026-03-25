using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.OrganizationInvites;

public sealed record OrganizationInviteDto(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string TargetEmail,
    OrganizationRole Role,
    DateTime ExpiresAt,
    Guid CreatedByUserId,
    Guid? AcceptedUserId,
    DateTime CreatedAt,
    DateTime? AcceptedAt,
    Guid? DeclinedUserId,
    DateTime? DeclinedAt);
