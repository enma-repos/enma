using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.ValueObjects;

/// <summary>
/// Unique key of a transition bucket (from_event -> to_event) for one project and one window.
/// </summary>
public sealed record EdgeKey
{
    public BucketBoundaryUtc BucketStartUtc { get; }
    public Guid OrganizationId { get; }
    public Guid ProjectId { get; }
    public Guid ProcessDefinitionId { get; }
    public EventName FromEvent { get; }
    public EventName ToEvent { get; }

    private EdgeKey(
        BucketBoundaryUtc bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        EventName fromEvent,
        EventName toEvent)
    {
        BucketStartUtc = bucketStartUtc;
        OrganizationId = organizationId;
        ProjectId = projectId;
        ProcessDefinitionId = processDefinitionId;
        FromEvent = fromEvent;
        ToEvent = toEvent;
    }

    public static Result<EdgeKey> Create(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string? fromEvent,
        string? toEvent)
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

        var fromEventVoResult = EventName.Create(fromEvent);
        if (fromEventVoResult.IsFailed)
        {
            errors.AddRange(fromEventVoResult.Errors);
        }

        var toEventVoResult = EventName.Create(toEvent);
        if (toEventVoResult.IsFailed)
        {
            errors.AddRange(toEventVoResult.Errors);
        }

        return errors.Count > 0
            ? Result.Fail<EdgeKey>(errors)
            : Result.Ok(new EdgeKey(
                bucketStartVoResult.Value,
                organizationId,
                projectId,
                processDefinitionId,
                fromEventVoResult.Value,
                toEventVoResult.Value));
    }

    public static EdgeKey Rehydrate(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string fromEvent,
        string toEvent)
        => new(
            BucketBoundaryUtc.Rehydrate(bucketStartUtc),
            organizationId,
            projectId,
            processDefinitionId,
            EventName.Rehydrate(fromEvent),
            EventName.Rehydrate(toEvent));
}
