using System.Text.Json.Nodes;
using Enma.Admin.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class Project
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }

    public string Name { get; private set; } = null!;
    public ProjectKey Key { get; private set; } 
    public string? Description { get; private set; }
    public bool IsStared { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public JsonObject? Settings { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? ArchivedAt { get; private set; }
    
    private Project() { }
    
    private Project(Guid id, Guid orgId, string name, ProjectKey key, string? description, bool isStared,
        Guid createdByUserId, JsonObject? settings, DateTime createdAt)
    {
        Id = id;
        OrganizationId = orgId;
        Name = name;
        Key = key;
        Description = description;
        IsStared = isStared;
        CreatedByUserId = createdByUserId;
        Settings = settings;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
    
    public static Result<Project> Create(Guid id, Guid orgId, string? name, string? key, string? description,
        bool isStared, Guid createdByUserId, JsonObject? settings, DateTime createdAt)
    {
        if (orgId == Guid.Empty)
        {
            return Result.Fail<Project>(ApplicationErrors.Required(nameof(orgId)));
        }

        if (createdByUserId == Guid.Empty)
        {
            return Result.Fail<Project>(ApplicationErrors.Required(nameof(createdByUserId)));
        }

        name = (name ?? string.Empty).Trim();
        if (name.Length is < 2 or > 200)
        {
            return Result.Fail<Project>(ApplicationErrors.Length(nameof(name), 2, 200));
        }
        
        if (description is not null && description.Length > 512)
        {
            return Result.Fail<Project>(ApplicationErrors.Length(nameof(description), 0, 512));
        }

        var keyRes = ProjectKey.Create(key);
        if (keyRes.IsFailed)
        {
            return Result.Fail<Project>(keyRes.Errors);
        }

        return Result.Ok(new Project(id, orgId, name, keyRes.Value, description, isStared, createdByUserId, settings, 
            createdAt));
    }
    
    public static Project Rehydrate(Guid id, Guid orgId, string name, string key, string? description, bool isStared,
        Guid createdByUserId, JsonObject? settings, DateTime createdAt, DateTime updatedAt, DateTime? archivedAt,
        DateTime? deletedAt)
    {
        return new Project
        {
            Id = id,
            OrganizationId = orgId,
            Name = name,
            Key = ProjectKey.Create(key).Value,
            Description = description,
            IsStared = isStared,
            CreatedByUserId = createdByUserId,
            Settings = settings,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            ArchivedAt = archivedAt,
            DeletedAt = deletedAt
        };
    }
}