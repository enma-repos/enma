using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class EventDefinitionsRepository : IEventDefinitionsRepository
{
    private readonly PostgresDbContext _context;

    public EventDefinitionsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<EventDefinition>> CreateAsync(EventDefinition model, Guid orgId, CancellationToken ct = default)
    {
        var projectBelongsToOrg = await _context.Projects
            .AnyAsync(p => p.Id == model.ProjectId && p.OrganizationId == orgId && p.DeletedAt == null, ct);

        if (!projectBelongsToOrg)
        {
            return Result.Fail<EventDefinition>(ApplicationErrors.EntityNotFound("Project", $"id={model.ProjectId}, orgId={orgId}"));
        }

        _context.EventDefinitions.Add(model.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(model);
    }

    public async Task<Result<EventDefinition>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.EventDefinitions
            .AsNoTracking()
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => new EventDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<EventDefinition>(ApplicationErrors.EntityNotFound("EventDefinition", $"id={id}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<EventDefinition>> GetByProjectAndNameAsync(Guid projectId, Guid orgId, string name,
        CancellationToken ct = default)
    {
        var entity = await _context.EventDefinitions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.Name == name && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => new EventDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<EventDefinition>(
                ApplicationErrors.EntityNotFound("EventDefinition", $"projectId={projectId}, name={name}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<EventDefinition>>> ListByProjectAsync(Guid projectId, Guid orgId, int offset,
        int limit, CancellationToken ct = default)
    {
        var entities = await _context.EventDefinitions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .OrderBy(x => x.Name)
            .Skip(offset)
            .Take(limit)
            .Select(x => new EventDefinitionEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Description = x.Description,
                CreatedByUserId = x.CreatedByUserId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<EventDefinition>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, string? description, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.EventDefinitions
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Description, description)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("EventDefinition", $"id={id}"))
            : Result.Ok();
    }

    public async Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.EventDefinitions
            .Where(x => x.Id == id && x.ProjectId == projectId && x.DeletedAt == null
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("EventDefinition", $"id={id}"))
            : Result.Ok();
    }
}
