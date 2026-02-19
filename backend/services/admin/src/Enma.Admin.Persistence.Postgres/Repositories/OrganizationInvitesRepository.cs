using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationInvitesRepository : IOrganizationInvitesRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationInvitesRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrganizationInvite>> CreateAsync(OrganizationInvite invite, CancellationToken ct = default)
    {
        _context.OrganizationInvites.Add(invite.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(invite);
    }

    public async Task<Result<OrganizationInvite>> GetByIdAsync(Guid inviteId, CancellationToken ct = default)
    {
        var entity = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x => x.Id == inviteId)
            .Select(x => new OrganizationInviteEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                TargetEmail = x.TargetEmail,
                Role = x.Role,
                TokenHash = x.TokenHash,
                ExpiresAt = x.ExpiresAt,
                CreatedByUserId = x.CreatedByUserId,
                AcceptedUserId = x.AcceptedUserId,
                CreatedAt = x.CreatedAt,
                AcceptedAt = x.AcceptedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<OrganizationInvite>(ApplicationErrors.EntityNotFound("OrganizationInvite", $"id={inviteId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<OrganizationInvite>> GetActiveByOrgAndEmailAsync(Guid orgId, string email, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var entity = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x =>
                x.OrganizationId == orgId &&
                x.TargetEmail == email &&
                x.AcceptedAt == null &&
                x.ExpiresAt > now)
            .Select(x => new OrganizationInviteEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                TargetEmail = x.TargetEmail,
                Role = x.Role,
                TokenHash = x.TokenHash,
                ExpiresAt = x.ExpiresAt,
                CreatedByUserId = x.CreatedByUserId,
                AcceptedUserId = x.AcceptedUserId,
                CreatedAt = x.CreatedAt,
                AcceptedAt = x.AcceptedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<OrganizationInvite>(ApplicationErrors.EntityNotFound(
                "OrganizationInvite",
                $"orgId={orgId}, email={email}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<OrganizationInvite>>> ListActiveByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var entities = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId && x.AcceptedAt == null && x.ExpiresAt > now)
            .Skip(offset)
            .Take(limit)
            .Select(x => new OrganizationInviteEntity
            {
                Id = x.Id,
                OrganizationId = x.OrganizationId,
                TargetEmail = x.TargetEmail,
                Role = x.Role,
                TokenHash = x.TokenHash,
                ExpiresAt = x.ExpiresAt,
                CreatedByUserId = x.CreatedByUserId,
                AcceptedUserId = x.AcceptedUserId,
                CreatedAt = x.CreatedAt,
                AcceptedAt = x.AcceptedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<OrganizationInvite>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetAcceptedAsync(Guid inviteId, Guid acceptedUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var state = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x => x.Id == inviteId)
            .Select(x => new { x.AcceptedAt, x.ExpiresAt })
            .FirstOrDefaultAsync(ct);

        if (state is null)
        {
            return Result.Fail(ApplicationErrors.EntityNotFound("OrganizationInvite", $"id={inviteId}"));
        }

        if (state.AcceptedAt is not null)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is already accepted.", code: "invite_accepted"));
        }

        if (state.ExpiresAt <= now)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is expired.", code: "invite_expired"));
        }

        var affected = await _context.OrganizationInvites
            .Where(x => x.Id == inviteId && x.AcceptedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.AcceptedAt, now)
                    .SetProperty(x => x.AcceptedUserId, acceptedUserId),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.Conflict("Invite cannot be accepted.", code: "invite_accept_failed")) 
            : Result.Ok();
    }
}
