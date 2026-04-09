using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services.Super;

internal sealed class SuperAuditLogsService : ISuperAuditLogsService
{
    private const int MaxPageSize = 200;

    private readonly IAuditLogsRepository _auditLogsRepository;

    public SuperAuditLogsService(IAuditLogsRepository auditLogsRepository)
    {
        _auditLogsRepository = auditLogsRepository;
    }

    public async Task<Result<PaginatedResult<AuditLogDto>>> ListAsync(
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        Guid? organizationId,
        Guid? projectId,
        int page,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var listRes = await _auditLogsRepository.ListAllAsync(
            from, to, action, resourceType, actorUserId, organizationId, projectId,
            page, pageSize, search, ct);
        if (listRes.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(listRes.Errors);

        var countRes = await _auditLogsRepository.CountAllAsync(
            from, to, action, resourceType, actorUserId, organizationId, projectId, search, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<AuditLogDto>>(countRes.Errors);

        var items = listRes.Value.Select(x => x.ToDto()).ToList();
        return PaginatedResult<AuditLogDto>.Create(items, countRes.Value, page, pageSize);
    }
}
