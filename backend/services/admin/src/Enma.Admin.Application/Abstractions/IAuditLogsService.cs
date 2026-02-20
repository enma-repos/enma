using Enma.Admin.Application.Dto.AuditLogs;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IAuditLogsService
{
    Task<Result> AppendAsync(CreateAuditLogDto dto, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AuditLogDto>>> ListByOrgAsync(
        Guid orgId,
        DateTime? from,
        DateTime? to,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<AuditLogDto>>> ListByProjectAsync(
        Guid projectId,
        DateTime? from,
        DateTime? to,
        int offset,
        int limit,
        CancellationToken ct = default);
}
