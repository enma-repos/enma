using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
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

    public async Task<Result<OrganizationInvite>> GetByIdAsync(Guid inviteId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.OrganizationInvites
            .AsNoTracking()
            .Include(x => x.Organization)
            .Where(x => x.Id == inviteId && x.OrganizationId == orgId)
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
            .Include(x => x.Organization)
            .Where(x =>
                x.OrganizationId == orgId &&
                x.TargetEmail == email &&
                x.AcceptedAt == null &&
                x.DeclinedAt == null &&
                x.ExpiresAt > now)
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
            .Include(x => x.Organization)
            .Where(x => x.OrganizationId == orgId && x.AcceptedAt == null && x.DeclinedAt == null && x.ExpiresAt > now)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<OrganizationInvite>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<IReadOnlyList<OrganizationInvite>>> ListPendingByEmailAsync(string email, int offset, int limit, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var entities = await _context.OrganizationInvites
            .AsNoTracking()
            .Include(x => x.Organization)
            .Where(x =>
                x.TargetEmail == email &&
                x.AcceptedAt == null &&
                x.DeclinedAt == null &&
                x.ExpiresAt > now)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<OrganizationInvite>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetAcceptedAsync(Guid inviteId, Guid orgId, Guid acceptedUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var state = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x => x.Id == inviteId && x.OrganizationId == orgId)
            .Select(x => new { x.AcceptedAt, x.DeclinedAt, x.ExpiresAt })
            .FirstOrDefaultAsync(ct);

        if (state is null)
        {
            return Result.Fail(ApplicationErrors.EntityNotFound("OrganizationInvite", $"id={inviteId}"));
        }

        if (state.AcceptedAt is not null)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is already accepted.", code: "invite_accepted"));
        }

        if (state.DeclinedAt is not null)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is already declined.", code: "invite_declined"));
        }

        if (state.ExpiresAt <= now)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is expired.", code: "invite_expired"));
        }

        var affected = await _context.OrganizationInvites
            .Where(x => x.Id == inviteId && x.OrganizationId == orgId && x.AcceptedAt == null && x.DeclinedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.AcceptedAt, now)
                    .SetProperty(x => x.AcceptedUserId, acceptedUserId),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.Conflict("Invite cannot be accepted.", code: "invite_accept_failed"))
            : Result.Ok();
    }

    public async Task<Result> SetDeclinedAsync(Guid inviteId, Guid orgId, Guid declinedUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var state = await _context.OrganizationInvites
            .AsNoTracking()
            .Where(x => x.Id == inviteId && x.OrganizationId == orgId)
            .Select(x => new { x.AcceptedAt, x.DeclinedAt, x.ExpiresAt })
            .FirstOrDefaultAsync(ct);

        if (state is null)
        {
            return Result.Fail(ApplicationErrors.EntityNotFound("OrganizationInvite", $"id={inviteId}"));
        }

        if (state.AcceptedAt is not null)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is already accepted.", code: "invite_accepted"));
        }

        if (state.DeclinedAt is not null)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is already declined.", code: "invite_declined"));
        }

        if (state.ExpiresAt <= now)
        {
            return Result.Fail(ApplicationErrors.Conflict("Invite is expired.", code: "invite_expired"));
        }

        var affected = await _context.OrganizationInvites
            .Where(x => x.Id == inviteId && x.OrganizationId == orgId && x.AcceptedAt == null && x.DeclinedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeclinedAt, now)
                    .SetProperty(x => x.DeclinedUserId, declinedUserId),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.Conflict("Invite cannot be declined.", code: "invite_decline_failed"))
            : Result.Ok();
    }
}
