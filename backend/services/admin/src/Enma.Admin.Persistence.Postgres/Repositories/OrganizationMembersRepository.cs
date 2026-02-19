using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using Enma.Common.Enums;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationMembersRepository : IOrganizationMembersRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationMembersRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrganizationMember>> AddAsync(OrganizationMember member, CancellationToken ct = default)
    {
        _context.OrganizationMembers.Add(member.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(member);
    }

    public async Task<Result<OrganizationMember>> GetAsync(Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var entity = await _context.OrganizationMembers
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId && x.UserId == userId)
            .Select(x => new OrganizationMemberEntity
            {
                OrganizationId = x.OrganizationId,
                UserId = x.UserId,
                Role = x.Role,
                Status = x.Status,
                JoinedAt = x.JoinedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<OrganizationMember>(ApplicationErrors.EntityNotFound("OrganizationMember", $"orgId={orgId}, userId={userId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<bool>> IsMemberAsync(Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var isMember = await _context.OrganizationMembers
            .AsNoTracking()
            .AnyAsync(x => x.OrganizationId == orgId && x.UserId == userId, ct);

        return Result.Ok(isMember);
    }

    public async Task<Result<IReadOnlyList<OrganizationMember>>> ListByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.OrganizationMembers
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId)
            .Skip(offset)
            .Take(limit)
            .Select(x => new OrganizationMemberEntity
            {
                OrganizationId = x.OrganizationId,
                UserId = x.UserId,
                Role = x.Role,
                Status = x.Status,
                JoinedAt = x.JoinedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<OrganizationMember>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetRoleAsync(Guid orgId, Guid userId, OrganizationRole role, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.OrganizationMembers
            .Where(x => x.OrganizationId == orgId && x.UserId == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Role, role)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("OrganizationMember", $"orgId={orgId}, userId={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetStatusAsync(Guid orgId, Guid userId, OrganizationMemberStatus status, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.OrganizationMembers
            .Where(x => x.OrganizationId == orgId && x.UserId == userId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Status, status)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("OrganizationMember", $"orgId={orgId}, userId={userId}")) 
            : Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var affected = await _context.OrganizationMembers
            .Where(x => x.OrganizationId == orgId && x.UserId == userId)
            .ExecuteDeleteAsync(ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("OrganizationMember", $"orgId={orgId}, userId={userId}"))
            : Result.Ok();
    }
}
