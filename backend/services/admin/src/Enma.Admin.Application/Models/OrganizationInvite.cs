using Enma.Admin.Application.ValueObjects;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class OrganizationInvite
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string OrganizationName { get; private set; } = "";

    public EmailAddress TargetEmail { get; private set; }
    public OrganizationRole Role { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public Guid CreatedByUserId { get; private set; }
    public Guid? AcceptedUserId { get; private set; }
    public Guid? DeclinedUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? DeclinedAt { get; private set; }

    private OrganizationInvite() { }

    private OrganizationInvite(Guid id, Guid orgId, EmailAddress email, OrganizationRole role,
        Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt, DateTime expiresAt, DateTime? acceptedAt,
        Guid? declinedUserId, DateTime? declinedAt)
    {
        Id = id;
        OrganizationId = orgId;
        TargetEmail = email;
        Role = role;
        CreatedByUserId = createdByUserId;
        AcceptedUserId = acceptedUserId;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        AcceptedAt = acceptedAt;
        DeclinedUserId = declinedUserId;
        DeclinedAt = declinedAt;
    }

    public static Result<OrganizationInvite> Create(Guid id, Guid orgId, string? email, OrganizationRole role,
        Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt, DateTime expiresAt,
        DateTime? acceptedAt)
    {
        if (acceptedAt < createdAt)
        {
            return Result.Fail<OrganizationInvite>(ApplicationErrors.Validation("Invalid accepted date."));
        }

        var emailResult = EmailAddress.Create(email);
        if (emailResult.IsFailed)
        {
            return Result.Fail<OrganizationInvite>(emailResult.Errors);
        }

        return Result.Ok(new OrganizationInvite(
            id, orgId, emailResult.Value, role, createdByUserId, acceptedUserId, createdAt, expiresAt,
            acceptedAt, null, null));
    }

    public static OrganizationInvite Rehydrate(Guid id, Guid orgId, string organizationName, string email,
        OrganizationRole role, Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt,
        DateTime expiresAt, DateTime? acceptedAt, Guid? declinedUserId, DateTime? declinedAt)
    {
        return new OrganizationInvite
        {
            Id = id,
            CreatedByUserId = createdByUserId,
            OrganizationId = orgId,
            OrganizationName = organizationName,
            Role = role,
            TargetEmail = EmailAddress.Create(email).Value,
            CreatedAt = createdAt,
            ExpiresAt = expiresAt,
            AcceptedUserId = acceptedUserId,
            AcceptedAt = acceptedAt,
            DeclinedUserId = declinedUserId,
            DeclinedAt = declinedAt
        };
    }
}
