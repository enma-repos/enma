using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.ProcessDefinitions;

public sealed record ProcessDefinitionDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Key,
    ProcessType Type,
    string? Description,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt);
