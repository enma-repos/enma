using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Admin.Persistence.Postgres.Mappers;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class AuditLogsRepository : IAuditLogsRepository
{
    private readonly PostgresDbContext _context;

    public AuditLogsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AppendAsync(AuditLog log, CancellationToken ct = default)
    {
        _context.AuditLogs.Add(log.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result<IReadOnlyList<AuditLog>>> ListByOrgAsync(
        Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = BuildOrgQuery(orgId, from, to, action, resourceType, actorUserId, search);

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ProjectEntity())
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AuditLog>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<int>> CountByOrgAsync(
        Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        string? search = null, CancellationToken ct = default)
    {
        var count = await BuildOrgQuery(orgId, from, to, action, resourceType, actorUserId, search).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<IReadOnlyList<AuditLog>>> ListByProjectAsync(
        Guid projectId, Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var query = BuildProjectQuery(projectId, orgId, from, to, action, resourceType, actorUserId, search);

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ProjectEntity())
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AuditLog>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<int>> CountByProjectAsync(
        Guid projectId, Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        string? search = null, CancellationToken ct = default)
    {
        var count = await BuildProjectQuery(projectId, orgId, from, to, action, resourceType, actorUserId, search).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<IReadOnlyList<AuditLog>>> ListAllAsync(
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        Guid? organizationId,
        Guid? projectId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        var query = BuildAllQuery(from, to, action, resourceType, actorUserId, organizationId, projectId, search);

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ProjectEntity())
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AuditLog>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<int>> CountAllAsync(
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        Guid? organizationId,
        Guid? projectId,
        string? search = null,
        CancellationToken ct = default)
    {
        var count = await BuildAllQuery(from, to, action, resourceType, actorUserId, organizationId, projectId, search).CountAsync(ct);
        return Result.Ok(count);
    }

    public async Task<Result<int>> CountRecentAllAsync(DateTime since, CancellationToken ct = default)
    {
        var count = await _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.CreatedAt >= since)
            .CountAsync(ct);
        return Result.Ok(count);
    }

    private IQueryable<AuditLogEntity> BuildAllQuery(
        DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        Guid? organizationId, Guid? projectId, string? search)
    {
        var query = _context.AuditLogs.AsNoTracking();

        if (organizationId is not null)
            query = query.Where(x => x.OrganizationId == organizationId.Value);

        if (projectId is not null)
            query = query.Where(x => x.ProjectId == projectId.Value);

        return ApplyFilters(query, from, to, action, resourceType, actorUserId, search);
    }

    private IQueryable<AuditLogEntity> BuildOrgQuery(
        Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId, string? search)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId);

        return ApplyFilters(query, from, to, action, resourceType, actorUserId, search);
    }

    private IQueryable<AuditLogEntity> BuildProjectQuery(
        Guid projectId, Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId, string? search)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && x.OrganizationId == orgId);

        return ApplyFilters(query, from, to, action, resourceType, actorUserId, search);
    }

    private static IQueryable<AuditLogEntity> ApplyFilters(
        IQueryable<AuditLogEntity> query,
        DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId, string? search)
    {
        if (from is not null)
            query = query.Where(x => x.CreatedAt >= from.Value);

        if (to is not null)
            query = query.Where(x => x.CreatedAt <= to.Value);

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(x => x.Action == action);

        if (!string.IsNullOrWhiteSpace(resourceType))
            query = query.Where(x => x.ResourceType == resourceType);

        if (actorUserId is not null)
            query = query.Where(x => x.ActorUserId == actorUserId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search}%";
            query = query.Where(x => EF.Functions.ILike(x.Action, term) || EF.Functions.ILike(x.ResourceType, term) || EF.Functions.ILike(x.ResourceId, term));
        }

        return query;
    }

    private static System.Linq.Expressions.Expression<Func<AuditLogEntity, AuditLogEntity>> ProjectEntity()
    {
        return x => new AuditLogEntity
        {
            Id = x.Id,
            OrganizationId = x.OrganizationId,
            ProjectId = x.ProjectId,
            ActorUserId = x.ActorUserId,
            Action = x.Action,
            ResourceType = x.ResourceType,
            ResourceId = x.ResourceId,
            Ip = x.Ip,
            Payload = x.Payload,
            CreatedAt = x.CreatedAt
        };
    }
}
