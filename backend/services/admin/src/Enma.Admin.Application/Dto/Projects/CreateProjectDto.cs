using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.Projects;

public sealed record CreateProjectDto(
    Guid OrganizationId,
    string? Name,
    string? Key,
    string? Description,
    bool IsStared,
    Guid CreatedByUserId,
    JsonObject? Settings);

