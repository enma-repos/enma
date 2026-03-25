using Enma.Admin.Application.Dto;
using Enma.Admin.Application.Dto.AuditLogs;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IAuditLogsService
{
    Task<Result> AppendAsync(CreateAuditLogDto dto, CancellationToken ct = default);

    Task<Result<PagedResult<AuditLogDto>>> ListByOrgAsync(
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<PagedResult<AuditLogDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        int offset,
        int limit,
        CancellationToken ct = default);
}
