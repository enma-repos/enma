using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface IAuditLogsRepository
{
    Task<Result> AppendAsync(AuditLog log, CancellationToken ct = default);

    Task<Result<IReadOnlyList<AuditLog>>> ListByOrgAsync(
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

    Task<Result<int>> CountByOrgAsync(
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        string? search = null,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<AuditLog>>> ListByProjectAsync(
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

    Task<Result<int>> CountByProjectAsync(
        Guid projectId,
        Guid orgId,
        DateTime? from,
        DateTime? to,
        string? action,
        string? resourceType,
        Guid? actorUserId,
        string? search = null,
        CancellationToken ct = default);
}
