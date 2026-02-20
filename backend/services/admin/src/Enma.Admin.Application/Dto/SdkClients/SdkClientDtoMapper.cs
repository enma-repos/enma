using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.SdkClients;

internal static class SdkClientDtoMapper
{
    internal static SdkClientDto ToDto(this SdkClient model)
        => new(
            Id: model.Id,
            ProjectId: model.ProjectId,
            Name: model.Name,
            Type: model.Type,
            Description: model.Description,
            Settings: model.Settings,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DisabledAt: model.DisabledAt);
}

