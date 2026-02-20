namespace Enma.Admin.Application.Dto.Organizations;

public sealed record CreateOrganizationDto(
    string? Name,
    string? Description,
    string Slug,
    Guid CreatedByUserId);

