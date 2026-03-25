using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class ProjectMember
{
    public Guid ProjectId { get; private set; }
    public Guid UserId { get; private set; }

    public ProjectRole Role { get; private set; }

    public string DisplayName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? AvatarUrl { get; private set; }

    public DateTime JoinedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ProjectMember() { }

    private ProjectMember(Guid projectId, Guid userId, ProjectRole role, DateTime joinedAt)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
        JoinedAt = joinedAt;
        UpdatedAt = joinedAt;
    }

    public static Result<ProjectMember> Create(Guid projectId, Guid userId, ProjectRole role, DateTime joinedAt)
        => Result.Ok(new ProjectMember(projectId, userId, role, joinedAt));
    
    internal static ProjectMember Rehydrate(Guid projectId, Guid userId, ProjectRole role, DateTime joinedAt)
        => new(projectId, userId, role, joinedAt);

    public static ProjectMember Rehydrate(Guid projectId, Guid userId, ProjectRole role,
        string displayName, string email, string? avatarUrl, DateTime joinedAt, DateTime updatedAt)
    {
        return new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId,
            Role = role,
            DisplayName = displayName,
            Email = email,
            AvatarUrl = avatarUrl,
            JoinedAt = joinedAt,
            UpdatedAt = updatedAt
        };
    }
}
