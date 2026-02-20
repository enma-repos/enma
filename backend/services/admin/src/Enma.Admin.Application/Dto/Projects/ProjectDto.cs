using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.Projects;

public sealed record ProjectDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Key,
    string? Description,
    bool IsStared,
    Guid CreatedByUserId,
    JsonObject? Settings,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt,
    DateTime? ArchivedAt);

