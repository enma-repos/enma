using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class OrganizationMember
{
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }

    public OrganizationRole Role { get; set; }
    public OrganizationMemberStatus Status { get; set; } = OrganizationMemberStatus.Active;

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
}