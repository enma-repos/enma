using Enma.Admin.Application.ValueObjects;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class ProcessDefinition
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = null!;
    public ProcessDefinitionKey Key { get; private set; }
    public ProcessType Type { get; private set; }
    public string? Description { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private ProcessDefinition() { }

    private ProcessDefinition(Guid id, Guid projectId, string name, ProcessDefinitionKey key, ProcessType type,
        string? description, Guid createdByUserId, DateTime createdAt)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        Key = key;
        Type = type;
        Description = description;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<ProcessDefinition> Create(Guid id, Guid projectId, string? name, string? key,
        ProcessType type, string? description, Guid createdByUserId, DateTime createdAt)
    {
        if (projectId == Guid.Empty)
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.Required(nameof(projectId)));
        }

        if (createdByUserId == Guid.Empty)
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.Required(nameof(createdByUserId)));
        }

        name = (name ?? string.Empty).Trim();
        if (name.Length is < 2 or > 200)
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.Length(nameof(name), 2, 200));
        }

        if (description is not null && description.Length > 512)
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.Length(nameof(description), 0, 512));
        }

        if (!Enum.IsDefined(type))
        {
            return Result.Fail<ProcessDefinition>(ApplicationErrors.Validation($"Invalid {nameof(type)} value."));
        }

        var keyRes = ProcessDefinitionKey.Create(key);
        if (keyRes.IsFailed)
        {
            return Result.Fail<ProcessDefinition>(keyRes.Errors);
        }

        return Result.Ok(new ProcessDefinition(id, projectId, name, keyRes.Value, type, description,
            createdByUserId, createdAt));
    }

    public static ProcessDefinition Rehydrate(Guid id, Guid projectId, string name, string key, ProcessType type,
        string? description, Guid createdByUserId, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt)
    {
        return new ProcessDefinition
        {
            Id = id,
            ProjectId = projectId,
            Name = name,
            Key = ProcessDefinitionKey.Create(key).Value,
            Type = type,
            Description = description,
            CreatedByUserId = createdByUserId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
}
