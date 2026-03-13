using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.ProcessDefinitions;

internal static class ProcessDefinitionDtoMapper
{
    internal static ProcessDefinitionDto ToDto(this ProcessDefinition model)
        => new(
            Id: model.Id,
            ProjectId: model.ProjectId,
            Name: model.Name,
            Key: model.Key.Value,
            Type: model.Type,
            Description: model.Description,
            CreatedByUserId: model.CreatedByUserId,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DeletedAt: model.DeletedAt);
}
