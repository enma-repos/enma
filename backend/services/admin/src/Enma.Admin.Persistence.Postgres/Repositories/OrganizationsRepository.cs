using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationsRepository : IOrganizationsRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Organization>> CreateAsync(Organization org, CancellationToken ct = default)
    {
        _context.Organizations.Add(org.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(org);
    }

    public async Task<Result<Organization>> GetByIdAsync(Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.Organizations
            .AsNoTracking()
            .Where(x => x.Id == orgId)
            .Select(x => new OrganizationEntity
            {
                Id = x.Id,
                OwnerUserId = x.OwnerUserId,
                CreatedByUserId = x.CreatedByUserId,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Organization>(ApplicationErrors.EntityNotFound("Organization", $"id={orgId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<Organization>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var entity = await _context.Organizations
            .AsNoTracking()
            .Where(x => x.Slug == slug)
            .Select(x => new OrganizationEntity
            {
                Id = x.Id,
                OwnerUserId = x.OwnerUserId,
                CreatedByUserId = x.CreatedByUserId,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Organization>(ApplicationErrors.EntityNotFound("Organization", $"slug={slug}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<Organization>>> ListByUserAsync(Guid userId, int offset, int limit, CancellationToken ct = default)
    {
        var entities = await _context.Organizations
            .AsNoTracking()
            .Where(o =>
                o.OwnerUserId == userId ||
                _context.OrganizationMembers.Any(m => m.OrganizationId == o.Id && m.UserId == userId))
            .Skip(offset)
            .Take(limit)
            .Select(x => new OrganizationEntity
            {
                Id = x.Id,
                OwnerUserId = x.OwnerUserId,
                CreatedByUserId = x.CreatedByUserId,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<Organization>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> UpdateAsync(Organization org, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Organizations
            .Where(x => x.Id == org.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, org.Name)
                    .SetProperty(x => x.Description, org.Description)
                    .SetProperty(x => x.OwnerUserId, org.OwnerUserId)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Organization", $"id={org.Id}"))
            : Result.Ok();
    }

    public async Task<Result> SetNameAsync(Guid orgId, string name, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Organizations
            .Where(x => x.Id == orgId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Organization", $"id={orgId}")) 
            : Result.Ok();
    }

    public async Task<Result> SetOwnerAsync(Guid orgId, Guid ownerUserId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Organizations
            .Where(x => x.Id == orgId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.OwnerUserId, ownerUserId)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0 
            ? Result.Fail(ApplicationErrors.EntityNotFound("Organization", $"id={orgId}")) 
            : Result.Ok();
    }

    public async Task<Result> SoftDeleteAsync(Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Organizations
            .Where(x => x.Id == orgId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Organization", $"id={orgId}"))
            : Result.Ok();
    }

    // ---------- Super-admin (platform-wide) ----------

    public async Task<Result<IReadOnlyList<SuperOrganizationListItemDto>>> ListSuperAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        var query = BuildSuperQuery(search, includeDeleted);

        var items = await query
            .OrderBy(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SuperOrganizationListItemDto(
                x.Id,
                x.Name,
                x.Slug,
                x.Description,
                x.OwnerUserId,
                _context.Users.Where(u => u.Id == x.OwnerUserId).Select(u => u.Email).FirstOrDefault(),
                _context.Users.Where(u => u.Id == x.OwnerUserId).Select(u => u.DisplayName).FirstOrDefault(),
                _context.OrganizationMembers.Count(m => m.OrganizationId == x.Id),
                _context.Projects.Count(p => p.OrganizationId == x.Id && p.DeletedAt == null),
                x.CreatedAt,
                x.DeletedAt))
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<SuperOrganizationListItemDto>>(items);
    }

    public async Task<Result<int>> CountSuperAsync(
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        var count = await BuildSuperQuery(search, includeDeleted).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<SuperOrganizationDetailsDto>> GetSuperDetailsAsync(Guid organizationId, CancellationToken ct = default)
    {
        var org = await _context.Organizations
            .AsNoTracking()
            .Where(x => x.Id == organizationId)
            .Select(x => new
            {
                x.Id, x.Name, x.Slug, x.Description, x.OwnerUserId,
                OwnerEmail = _context.Users.Where(u => u.Id == x.OwnerUserId).Select(u => u.Email).FirstOrDefault(),
                OwnerDisplayName = _context.Users.Where(u => u.Id == x.OwnerUserId).Select(u => u.DisplayName).FirstOrDefault(),
                x.CreatedAt, x.UpdatedAt, x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        if (org is null)
        {
            return Result.Fail<SuperOrganizationDetailsDto>(ApplicationErrors.EntityNotFound("Organization", $"id={organizationId}"));
        }

        var members = await _context.OrganizationMembers
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId)
            .Join(_context.Users,
                m => m.UserId,
                u => u.Id,
                (m, u) => new SuperOrganizationMemberDto(
                    u.Id,
                    u.Email,
                    u.DisplayName,
                    u.AvatarUrl,
                    m.Role,
                    m.JoinedAt))
            .OrderBy(x => x.Email)
            .ToListAsync(ct);

        var projects = await _context.Projects
            .AsNoTracking()
            .Where(p => p.OrganizationId == organizationId)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new SuperOrganizationProjectDto(
                p.Id,
                p.Name,
                p.Key,
                _context.ProjectMembers.Count(m => m.ProjectId == p.Id),
                p.CreatedAt,
                p.DeletedAt,
                p.ArchivedAt))
            .ToListAsync(ct);

        return Result.Ok(new SuperOrganizationDetailsDto(
            org.Id,
            org.Name,
            org.Slug,
            org.Description,
            org.OwnerUserId,
            org.OwnerEmail,
            org.OwnerDisplayName,
            org.CreatedAt,
            org.UpdatedAt,
            org.DeletedAt,
            members,
            projects));
    }

    public async Task<Result<int>> CountAllAsync(bool includeDeleted, CancellationToken ct = default)
    {
        var query = _context.Organizations.AsNoTracking();
        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        var count = await query.CountAsync(ct);
        return Result.Ok(count);
    }

    private IQueryable<OrganizationEntity> BuildSuperQuery(string? search, bool includeDeleted)
    {
        var query = _context.Organizations.AsNoTracking();

        if (!includeDeleted)
        {
            query = query.Where(x => x.DeletedAt == null);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, term) ||
                EF.Functions.ILike(x.Slug, term));
        }

        return query;
    }
}
