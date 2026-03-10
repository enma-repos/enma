using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Unique key of an event-node bucket for one project and one window.
/// </summary>
public sealed class NodeKey
{
    public BucketBoundaryUtc BucketStartUtc { get; }
    public Guid OrganizationId { get; }
    public Guid ProjectId { get; }
    public Guid ProcessDefinitionId { get; }
    public EventName EventName { get; }

    private NodeKey(
        BucketBoundaryUtc bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        EventName eventName)
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

        var bucketStartVoResult = BucketBoundaryUtc.Create(bucketStartUtc);
        if (bucketStartVoResult.IsFailed)
        {
            errors.AddRange(bucketStartVoResult.Errors);
        }

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

        var eventNameVoResult = EventName.Create(eventName);
        if (eventNameVoResult.IsFailed)
        {
            errors.AddRange(eventNameVoResult.Errors);
        }

        return errors.Count > 0
            ? Result.Fail<NodeKey>(errors)
            : Result.Ok(new NodeKey(
                bucketStartVoResult.Value,
                organizationId,
                projectId,
                processDefinitionId,
                eventNameVoResult.Value));
    }

    public static NodeKey Rehydrate(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string eventName)
        => new(
            BucketBoundaryUtc.Rehydrate(bucketStartUtc),
            organizationId,
            projectId,
            processDefinitionId,
            EventName.Rehydrate(eventName));
}
