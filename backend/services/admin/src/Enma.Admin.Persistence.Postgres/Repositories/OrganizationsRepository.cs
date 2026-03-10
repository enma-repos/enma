using Enma.Admin.Application.Contracts;
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
}
