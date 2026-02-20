namespace Enma.Admin.Application.Dto.Organizations;

public sealed record OrganizationDto(
    Guid Id,
    Guid OwnerUserId,
    Guid CreatedByUserId,
    string Name,
    string Slug,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt);

