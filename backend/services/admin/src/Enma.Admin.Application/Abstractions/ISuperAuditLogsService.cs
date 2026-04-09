using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface ISuperAuditLogsService
{
    Task<Result<PaginatedResult<AuditLogDto>>> ListAsync(
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
        CancellationToken ct = default);
}
