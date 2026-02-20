using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using System.Net;
using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Admin.Application.Models;
using Enma.Common.Errors;
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

    public async Task<Result<IReadOnlyList<AuditLogDto>>> ListByOrgAsync(
        Guid orgId, 
        DateTime? from, 
        DateTime? to, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _auditLogsRepository.ListByOrgAsync(orgId, from, to, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<AuditLogDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<AuditLogDto>>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<AuditLogDto>>> ListByProjectAsync(
        Guid projectId, 
        DateTime? from, 
        DateTime? to, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _auditLogsRepository.ListByProjectAsync(projectId, from, to, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<AuditLogDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<AuditLogDto>>(res.Errors);
    }
}
