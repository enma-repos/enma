using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Unique key of an event-node bucket for one project and one window.
/// </summary>
public sealed class NodeKey
{
    public DateTime BucketStartUtc { get; }
    public Guid OrganizationId { get; }
    public Guid ProjectId { get; }
    public Guid ProcessDefinitionId { get; }
    public string EventName { get; }

    private NodeKey(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string eventName)
    {
        BucketStartUtc = bucketStartUtc;
        OrganizationId = organizationId;
        ProjectId = projectId;
        ProcessDefinitionId = processDefinitionId;
        EventName = eventName;
    }

    public static Result<NodeKey> Create(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string? eventName)
    {
        var errors = new List<IError>();

        ModelValidation.AddUtcDateTime(errors, bucketStartUtc, nameof(bucketStartUtc));
        if (!ModelValidation.IsFiveMinuteBoundary(bucketStartUtc))
        {
            errors.Add(ApplicationErrors.Validation("bucketStartUtc must be aligned to a 5-minute boundary."));
        }

        ModelValidation.AddRequiredGuid(errors, organizationId, nameof(organizationId));
        ModelValidation.AddRequiredGuid(errors, projectId, nameof(projectId));
        ModelValidation.AddRequiredGuid(errors, processDefinitionId, nameof(processDefinitionId));

        var normalizedEventName = ModelValidation.ValidateRequiredString(
            errors,
            eventName,
            nameof(eventName),
            minLength: 1,
            maxLength: 200);

        return errors.Count > 0
            ? Result.Fail<NodeKey>(errors)
            : Result.Ok(new NodeKey(
                bucketStartUtc,
                organizationId,
                projectId,
                processDefinitionId,
                normalizedEventName));
    }

    public static NodeKey Rehydrate(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string eventName)
        => new NodeKey(
            bucketStartUtc,
            organizationId,
            projectId,
            processDefinitionId,
            eventName);
}