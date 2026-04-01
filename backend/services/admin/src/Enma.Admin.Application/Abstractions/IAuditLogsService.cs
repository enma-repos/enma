using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IAuditLogsService
{
    Task<Result> AppendAsync(CreateAuditLogDto dto, CancellationToken ct = default);

    Task<Result<PaginatedResult<AuditLogDto>>> ListByOrgAsync(
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default);

    Task<Result<PaginatedResult<AuditLogDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default);
}
