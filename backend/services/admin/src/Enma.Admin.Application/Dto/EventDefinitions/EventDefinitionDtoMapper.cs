using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.EventDefinitions;

internal static class EventDefinitionDtoMapper
{
    internal static EventDefinitionDto ToDto(this EventDefinition model)
        => new(
            Id: model.Id,
            ProjectId: model.ProjectId,
            Name: model.Name,
            Description: model.Description,
            CreatedByUserId: model.CreatedByUserId,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DeletedAt: model.DeletedAt);
}
