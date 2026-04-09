using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.Super;

public sealed record SuperProjectMemberDto(
    Guid UserId,
    string Email,
    string DisplayName,
    string? AvatarUrl,
    ProjectRole Role,
    DateTime JoinedAt);

public sealed record SuperProjectDetailsDto(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    Guid OrganizationId,
    string OrganizationName,
    string OrganizationSlug,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt,
    DateTime? ArchivedAt,
    int SdkClientCount,
    int ProcessDefinitionCount,
    int EventDefinitionCount,
    IReadOnlyList<SuperProjectMemberDto> Members);
