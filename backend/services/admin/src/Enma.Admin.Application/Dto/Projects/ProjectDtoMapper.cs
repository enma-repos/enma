using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.Projects;

internal static class ProjectDtoMapper
{
    internal static ProjectDto ToDto(this Project model)
        => new(
            Id: model.Id,
            OrganizationId: model.OrganizationId,
            Name: model.Name,
            Key: model.Key.Value,
            Description: model.Description,
            IsStared: model.IsStared,
            CreatedByUserId: model.CreatedByUserId,
            Settings: model.Settings,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DeletedAt: model.DeletedAt,
            ArchivedAt: model.ArchivedAt);
}

