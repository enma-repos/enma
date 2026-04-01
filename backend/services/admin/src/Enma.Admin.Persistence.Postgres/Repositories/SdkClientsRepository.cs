using System.Text.Json.Nodes;
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

internal sealed class SdkClientsRepository : ISdkClientsRepository
{
    private readonly PostgresDbContext _context;

    public SdkClientsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SdkClient>> CreateAsync(SdkClient client, Guid orgId, CancellationToken ct = default)
    {
        var projectBelongsToOrg = await _context.Projects
            .AnyAsync(p => p.Id == client.ProjectId && p.OrganizationId == orgId && p.DeletedAt == null, ct);

        if (!projectBelongsToOrg)
        {
            return Result.Fail<SdkClient>(ApplicationErrors.EntityNotFound("Project", $"id={client.ProjectId}, orgId={orgId}"));
        }

        _context.ApiClients.Add(client.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(client);
    }

    public async Task<Result<SdkClient>> GetByIdAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var entity = await _context.ApiClients
            .AsNoTracking()
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .Select(x => new SdkClientEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Type = x.Type,
                Description = x.Description,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DisabledAt = x.DisabledAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<SdkClient>(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<IReadOnlyList<SdkClient>>> ListByProjectAsync(Guid projectId, Guid orgId, int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ApiClients
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, term) || EF.Functions.ILike(x.Description ?? "", term));
        }

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SdkClientEntity
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                Type = x.Type,
                Description = x.Description,
                Settings = x.Settings,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DisabledAt = x.DisabledAt
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<SdkClient>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<int>> CountByProjectAsync(Guid projectId, Guid orgId, string? search = null, CancellationToken ct = default)
    {
        var query = _context.ApiClients
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, term) || EF.Functions.ILike(x.Description ?? "", term));
        }

        var count = await query.CountAsync(ct);

        return Result.Ok(count);
    }

    public async Task<Result> UpdateAsync(SdkClient client, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == client.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, client.Name)
                    .SetProperty(x => x.Description, client.Description)
                    .SetProperty(x => x.Type, client.Type)
                    .SetProperty(x => x.Settings, client.Settings)
                    .SetProperty(x => x.DisabledAt, client.DisabledAt)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={client.Id}"))
            : Result.Ok();
    }

    public async Task<Result> SetNameAsync(Guid clientId, Guid projectId, Guid orgId, string name, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Name, name)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }

    public async Task<Result> SetDescriptionAsync(Guid clientId, Guid projectId, Guid orgId, string? description, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Description, description)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }

    public async Task<Result> SetSettingsAsync(Guid clientId, Guid projectId, Guid orgId, JsonObject? settings, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Settings, settings)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }

    public async Task<Result> SetTypeAsync(Guid clientId, Guid projectId, Guid orgId, SdkClientType type, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Type, type)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }

    public async Task<Result> SetDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DisabledAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }

    public async Task<Result> ClearDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ApiClients
            .Where(x => x.Id == clientId && x.ProjectId == projectId
                && _context.Projects.Any(p => p.Id == projectId && p.OrganizationId == orgId))
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.DisabledAt, (DateTime?)null)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("SdkClient", $"id={clientId}"))
            : Result.Ok();
    }
}
