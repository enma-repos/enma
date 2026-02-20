using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.Organizations;

internal static class OrganizationDtoMapper
{
    internal static OrganizationDto ToDto(this Organization model)
        => new(
            Id: model.Id,
            OwnerUserId: model.OwnerUserId,
            CreatedByUserId: model.CreatedByUserId,
            Name: model.Name,
            Slug: model.Slug.Value,
            Description: model.Description,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DeletedAt: model.DeletedAt);
}

