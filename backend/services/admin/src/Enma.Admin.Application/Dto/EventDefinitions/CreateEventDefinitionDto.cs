namespace Enma.Admin.Application.Dto.EventDefinitions;

public sealed record CreateEventDefinitionDto(
    Guid ProjectId,
    string? Name,
    string? Description,
    Guid CreatedByUserId);
