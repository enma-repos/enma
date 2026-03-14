using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class EventDefinition
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private EventDefinition() { }

    private EventDefinition(Guid id, Guid projectId, string name,
        string? description, Guid createdByUserId, DateTime createdAt)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<EventDefinition> Create(Guid id, Guid projectId, string? name,
        string? description, Guid createdByUserId, DateTime createdAt)
    {
        if (projectId == Guid.Empty)
        {
            return Result.Fail<EventDefinition>(ApplicationErrors.Required(nameof(projectId)));
        }

        if (createdByUserId == Guid.Empty)
        {
            return Result.Fail<EventDefinition>(ApplicationErrors.Required(nameof(createdByUserId)));
        }

        name = (name ?? string.Empty).Trim();
        if (name.Length is < 2 or > 200)
        {
            return Result.Fail<EventDefinition>(ApplicationErrors.Length(nameof(name), 2, 200));
        }

        if (description is not null && description.Length > 512)
        {
            return Result.Fail<EventDefinition>(ApplicationErrors.Length(nameof(description), 0, 512));
        }

        return Result.Ok(new EventDefinition(id, projectId, name, description,
            createdByUserId, createdAt));
    }

    public static EventDefinition Rehydrate(Guid id, Guid projectId, string name,
        string? description, Guid createdByUserId, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt)
    {
        return new EventDefinition
        {
            Id = id,
            ProjectId = projectId,
            Name = name,
            Description = description,
            CreatedByUserId = createdByUserId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
}
