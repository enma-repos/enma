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

    public async Task<Result<IReadOnlyList<AuditLog>>> ListByOrgAsync(Guid orgId, DateTime? from, DateTime? to, int offset, int limit, CancellationToken ct = default)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.OrganizationId == orgId);

        if (from is not null)
        {
            query = query.Where(x => x.CreatedAt >= from.Value);
        }

        if (to is not null)
        {
            query = query.Where(x => x.CreatedAt <= to.Value);
        }

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(x => new AuditLogEntity
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
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AuditLog>>(entities.Select(x => x.ToModel()).ToList());
    }

    public async Task<Result<IReadOnlyList<AuditLog>>> ListByProjectAsync(Guid projectId, DateTime? from, DateTime? to, int offset, int limit,
        CancellationToken ct = default)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId);

        if (from is not null)
        {
            query = query.Where(x => x.CreatedAt >= from.Value);
        }

        if (to is not null)
        {
            query = query.Where(x => x.CreatedAt <= to.Value);
        }

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(x => new AuditLogEntity
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
            })
            .ToListAsync(ct);

        return Result.Ok<IReadOnlyList<AuditLog>>(entities.Select(x => x.ToModel()).ToList());
    }
}
