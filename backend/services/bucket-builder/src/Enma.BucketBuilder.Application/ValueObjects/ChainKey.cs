using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Stable chain identifier for path stitching:
/// organization + project + process definition + process instance.
/// </summary>
public readonly record struct ChainKey(
    Guid OrganizationId,
    Guid ProjectId,
    Guid ProcessDefinitionId,
    ProcessId ProcessId)
{
    public static Result<ChainKey> Create(
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string? processId)
    {
        var errors = new List<IError>();

        if (organizationId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(organizationId)));
        }

        if (projectId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(projectId)));
        }

        if (processDefinitionId == Guid.Empty)
        {
            errors.Add(ApplicationErrors.Required(nameof(processDefinitionId)));
        }

        var processIdVoResult = ProcessId.Create(processId);
        if (processIdVoResult.IsFailed)
        {
            errors.AddRange(processIdVoResult.Errors);
        }

        return errors.Count > 0
            ? Result.Fail<ChainKey>(errors)
            : Result.Ok(new ChainKey(
                organizationId,
                projectId,
                processDefinitionId,
                processIdVoResult.Value));
    }

    public static ChainKey Rehydrate(
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string processId)
        => new(organizationId, projectId, processDefinitionId, ProcessId.Rehydrate(processId));

    public string ToStorageKey()
    {
        return $"{OrganizationId:N}:{ProjectId:N}:{ProcessDefinitionId:N}:{ProcessId}";
    }
}
