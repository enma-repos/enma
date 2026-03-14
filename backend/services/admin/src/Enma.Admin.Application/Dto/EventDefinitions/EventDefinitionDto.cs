namespace Enma.Admin.Application.Dto.EventDefinitions;

public sealed record EventDefinitionDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string? Description,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt);
