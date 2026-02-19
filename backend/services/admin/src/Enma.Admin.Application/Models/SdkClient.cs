using System.Text.Json.Nodes;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class SdkClient
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = null!;
    public SdkClientType Type { get; private set; }
    public string? Description { get; private set; }

    public JsonObject? Settings { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DisabledAt { get; private set; }
    
    private SdkClient() { }

    private SdkClient(Guid id, Guid projectId, string name, string? description, SdkClientType type, 
        JsonObject? settings, DateTime createdAt)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        Description = description;
        Type = type;
        Settings = settings;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
    
    public static Result<SdkClient> Create(Guid id, Guid projectId, string? name, string? description,
        SdkClientType type, JsonObject? settings, DateTime createdAt)
    {
        if (projectId == Guid.Empty)
        {
            return Result.Fail<SdkClient>(ApplicationErrors.Required(nameof(projectId)));
        }

        name = (name ?? string.Empty).Trim();
        if (name.Length is < 2 or > 200)
        {
            return Result.Fail<SdkClient>(ApplicationErrors.Length(nameof(name), 2, 200));
        }
        
        if (description is not null && description.Length > 512)
        {
            return Result.Fail<SdkClient>(ApplicationErrors.Length(nameof(description), 0, 512));
        }

        return Result.Ok(new SdkClient(id, projectId, name, description, type, settings, createdAt));
    }
    
    internal static SdkClient Rehydrate(Guid id, Guid projectId, string name, string? description, SdkClientType type,
        JsonObject? settings, DateTime createdAt, DateTime updatedAt, DateTime? disabledAt)
    {
        return new SdkClient
        {
            Id = id,
            ProjectId = projectId,
            Name = name,
            Description = description,
            Type = type,
            Settings = settings,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DisabledAt = disabledAt
        };
    }

}