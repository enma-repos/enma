using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using System.Net;
using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Admin.Application.Models;
using Enma.Common.Errors;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class AuditLogsService : IAuditLogsService
{
    private readonly IAuditLogsRepository _auditLogsRepository;

    public AuditLogsService(IAuditLogsRepository auditLogsRepository)
    {
        _auditLogsRepository = auditLogsRepository;
    }

    public async Task<Result> AppendAsync(
        CreateAuditLogDto dto,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        IPAddress? ip = null;

        if (!string.IsNullOrWhiteSpace(dto.Ip) && !IPAddress.TryParse(dto.Ip, out ip))
        {
            return Result.Fail(ApplicationErrors.InvalidFormat(nameof(dto.Ip)));
        }

        var modelRes = AuditLog.Create(
            id: Guid.NewGuid(),
            orgId: dto.OrganizationId,
            projectId: dto.ProjectId,
            actorUserId: dto.ActorUserId,
            action: dto.Action,
            resourceType: dto.ResourceType,
            resourceId: dto.ResourceId,
            ip: ip,
            payload: dto.Payload,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail(modelRes.Errors);
        }

        return await _auditLogsRepository.AppendAsync(modelRes.Value, ct);
    }

    public async Task<Result<PaginatedResult<AuditLogDto>>> ListByOrgAsync(
        Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var res = await _auditLogsRepository.ListByOrgAsync(orgId, from, to, action, resourceType, actorUserId, page, pageSize, search, ct);
        if (res.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(res.Errors);

        var countRes = await _auditLogsRepository.CountByOrgAsync(orgId, from, to, action, resourceType, actorUserId, search, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(countRes.Errors);

        var items = res.Value.Select(x => x.ToDto()).ToList();
        var paginatedResult = PaginatedResult<AuditLogDto>.Create(items, countRes.Value, page, pageSize);
        return paginatedResult;
    }

    public async Task<Result<PaginatedResult<AuditLogDto>>> ListByProjectAsync(
        Guid projectId, Guid orgId, DateTime? from, DateTime? to,
        string? action, string? resourceType, Guid? actorUserId,
        int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        var res = await _auditLogsRepository.ListByProjectAsync(projectId, orgId, from, to, action, resourceType, actorUserId, page, pageSize, search, ct);
        if (res.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(res.Errors);

        var countRes = await _auditLogsRepository.CountByProjectAsync(projectId, orgId, from, to, action, resourceType, actorUserId, search, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(countRes.Errors);

        var items = res.Value.Select(x => x.ToDto()).ToList();
        var paginatedResult = PaginatedResult<AuditLogDto>.Create(items, countRes.Value, page, pageSize);
        return paginatedResult;
    }
}
