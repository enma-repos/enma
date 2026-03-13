using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.ProcessDefinitions;

public sealed record CreateProcessDefinitionDto(
    Guid ProjectId,
    string? Name,
    string? Key,
    ProcessType Type,
    string? Description,
    Guid CreatedByUserId);
