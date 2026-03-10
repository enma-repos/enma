using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Stable chain identifier for path stitching:
/// organization + project + process definition + process instance.
/// </summary>
public sealed class ChainKey
{
    public Guid OrganizationId { get; }
    public Guid ProjectId { get; }
    public Guid ProcessDefinitionId { get; }
    public string ProcessId { get; }

    private ChainKey(
        Guid organizationId, 
        Guid projectId,
        Guid processDefinitionId,
        string processId)
    {
        OrganizationId = organizationId;
        ProjectId = projectId;
        ProcessDefinitionId = processDefinitionId;
        ProcessId = processId;
    }

    public static Result<ChainKey> Create(
        Guid organizationId, 
        Guid projectId, 
        Guid processDefinitionId,
        string? processId)
    {
        var errors = new List<IError>();

        ModelValidation.AddRequiredGuid(errors, organizationId, nameof(organizationId));
        ModelValidation.AddRequiredGuid(errors, projectId, nameof(projectId));
        ModelValidation.AddRequiredGuid(errors, processDefinitionId, nameof(processDefinitionId));

        var normalizedProcessId = ModelValidation.ValidateRequiredString(
            errors,
            processId,
            nameof(processId),
            minLength: 1,
            maxLength: 256);

        return errors.Count > 0
            ? Result.Fail<ChainKey>(errors)
            : Result.Ok(new ChainKey(organizationId, projectId, processDefinitionId, normalizedProcessId));
    }

    public static ChainKey Rehydrate(
        Guid organizationId, 
        Guid projectId, 
        Guid processDefinitionId, 
        string processId)
        => new (organizationId, projectId, processDefinitionId, processId);

    public string ToStorageKey() => $"{OrganizationId:N}:{ProjectId:N}:{ProcessDefinitionId:N}:{ProcessId}";
}