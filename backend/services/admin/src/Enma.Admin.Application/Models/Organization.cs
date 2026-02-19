using Enma.Admin.Application.ValueTypes;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class Organization
{
    public Guid Id { get; private set; }
    
    public Guid OwnerUserId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    
    public OrganizationSlug Slug { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    private Organization() { }

    private Organization(Guid id, string name, string? description, OrganizationSlug slug, Guid ownerUserId,
        Guid createdByUserId, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Slug = slug;
        OwnerUserId = ownerUserId;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<Organization> Create(Guid id, string? name, string? description, string slug, Guid ownerUserId, 
        Guid createdByUserId, DateTime createdAt)
    {
        name = (name ?? string.Empty).Trim();
        if (name.Length is < 2 or > 200)
        {
            return Result.Fail<Organization>(ApplicationErrors.Length("Organization name", 2, 200));
        }
        
        if (description is not null && description.Length > 512)
        {
            return Result.Fail<Organization>(ApplicationErrors.Length("Organization description", 0, 512));
        }

        if (ownerUserId == Guid.Empty)
        {
            return Result.Fail<Organization>(ApplicationErrors.Required("Owner userId"));
        }

        var slugResult = OrganizationSlug.Create(slug);
        if (slugResult.IsFailed)
        {
            return Result.Fail<Organization>(slugResult.Errors);
        }

        return Result.Ok(new Organization(id, name, description, slugResult.Value, ownerUserId, createdByUserId, createdAt));
    }

    public static Organization Rehydrate(Guid id, string name, string? description, string slug, 
        Guid ownerUserId, Guid createdByUserId, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt)
    {
        return new Organization
        {
            Id = id,
            Name = name,
            Description = description,
            Slug = OrganizationSlug.Create(slug).Value,
            OwnerUserId = ownerUserId,
            CreatedByUserId = createdByUserId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
}