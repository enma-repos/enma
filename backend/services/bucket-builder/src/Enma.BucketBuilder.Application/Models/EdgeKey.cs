using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Unique key of a transition bucket (from_event -> to_event) for one project and one window.
/// </summary>
public sealed class EdgeKey
{
    public DateTime BucketStartUtc { get; }
    public Guid OrganizationId { get; }
    public Guid ProjectId { get; }
    public Guid ProcessDefinitionId { get; }
    public string FromEvent { get; }
    public string ToEvent { get; }

    private EdgeKey(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string fromEvent,
        string toEvent)
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

        ModelValidation.AddUtcDateTime(errors, bucketStartUtc, nameof(bucketStartUtc));
        if (!ModelValidation.IsFiveMinuteBoundary(bucketStartUtc))
        {
            errors.Add(ApplicationErrors.Validation(
                "bucketStartUtc must be aligned to a 5-minute boundary."));
        }

        ModelValidation.AddRequiredGuid(errors, organizationId, nameof(organizationId));
        ModelValidation.AddRequiredGuid(errors, projectId, nameof(projectId));
        ModelValidation.AddRequiredGuid(errors, processDefinitionId, nameof(processDefinitionId));

        var normalizedFromEvent = ModelValidation.ValidateRequiredString(
            errors,
            fromEvent,
            nameof(fromEvent),
            minLength: 1,
            maxLength: 200);

        var normalizedToEvent = ModelValidation.ValidateRequiredString(
            errors,
            toEvent,
            nameof(toEvent),
            minLength: 1,
            maxLength: 200);

        return errors.Count > 0
            ? Result.Fail<EdgeKey>(errors)
            : Result.Ok(new EdgeKey(
                bucketStartUtc,
                organizationId,
                projectId,
                processDefinitionId,
                normalizedFromEvent,
                normalizedToEvent));
    }

    public static EdgeKey Rehydrate(
        DateTime bucketStartUtc,
        Guid organizationId,
        Guid projectId,
        Guid processDefinitionId,
        string fromEvent,
        string toEvent)
        => new(
            bucketStartUtc,
            organizationId,
            projectId,
            processDefinitionId,
            fromEvent,
            toEvent);
}