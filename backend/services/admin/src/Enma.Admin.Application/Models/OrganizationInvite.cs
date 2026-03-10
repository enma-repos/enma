using Enma.Admin.Application.ValueObjects;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class OrganizationInvite
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    
    public EmailAddress TargetEmail { get; private set; }
    public OrganizationRole Role { get; private set; }

    public string TokenHash { get; private set; } = null!;

    public DateTime ExpiresAt { get; private set; }

    public Guid CreatedByUserId { get; private set; }
    public Guid? AcceptedUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    
    private OrganizationInvite() { }
    
    private OrganizationInvite(Guid id, Guid orgId, EmailAddress email, OrganizationRole role, string tokenHash, 
        Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt, DateTime expiresAt, DateTime? acceptedAt)
    {
        Id = id;
        OrganizationId = orgId;
        TargetEmail = email;
        Role = role;
        TokenHash = tokenHash;
        CreatedByUserId = createdByUserId;
        AcceptedUserId = acceptedUserId;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        AcceptedAt = acceptedAt;
    }
    
    public static Result<OrganizationInvite> Create(Guid id, Guid orgId, string? email, OrganizationRole role, 
        string? tokenHash, Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt, DateTime expiresAt, 
        DateTime? acceptedAt)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
        {
            return Result.Fail<OrganizationInvite>(ApplicationErrors.Required(nameof(tokenHash)));
        }

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
            id, orgId, emailResult.Value, role, tokenHash, createdByUserId, acceptedUserId, createdAt, expiresAt, 
            acceptedAt));
    }
    
    public static OrganizationInvite Rehydrate(Guid id, Guid orgId, string email, OrganizationRole role, 
        string tokenHash, Guid createdByUserId, Guid? acceptedUserId, DateTime createdAt, DateTime expiresAt, 
        DateTime? acceptedAt)
    {
        return new OrganizationInvite
        {
            Id = id,
            CreatedByUserId = createdByUserId,
            OrganizationId = orgId,
            TokenHash = tokenHash,
            Role = role,
            TargetEmail = EmailAddress.Create(email).Value,
            CreatedAt = createdAt,
            ExpiresAt = expiresAt,
            AcceptedUserId = acceptedUserId,
            AcceptedAt = acceptedAt
        };
    }
}