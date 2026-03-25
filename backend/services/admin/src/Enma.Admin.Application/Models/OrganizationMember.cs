using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class OrganizationMember
{
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }

    public OrganizationRole Role { get; set; }
    public OrganizationMemberStatus Status { get; set; } = OrganizationMemberStatus.Active;

    public string DisplayName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? AvatarUrl { get; private set; }

    public DateTime JoinedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private OrganizationMember() { }

    private OrganizationMember(Guid organizationId, Guid userId, OrganizationRole role, DateTime joinedAt)
    {
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
        JoinedAt = joinedAt;
        UpdatedAt = joinedAt;
    }

    public static Result<OrganizationMember> Create(Guid orgId, Guid userId, OrganizationRole role, DateTime now)
        => Result.Ok(new OrganizationMember(orgId, userId, role, now));

    internal static OrganizationMember Rehydrate(Guid orgId, Guid userId, OrganizationRole role, DateTime now)
        => new(orgId, userId, role, now);

    public static OrganizationMember Rehydrate(Guid orgId, Guid userId, OrganizationRole role,
        OrganizationMemberStatus status, string displayName, string email, string? avatarUrl,
        DateTime joinedAt, DateTime updatedAt)
    {
        return new OrganizationMember
        {
            OrganizationId = orgId,
            UserId = userId,
            Role = role,
            Status = status,
            DisplayName = displayName,
            Email = email,
            AvatarUrl = avatarUrl,
            JoinedAt = joinedAt,
            UpdatedAt = updatedAt
        };
    }
}
