using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="AuditLog"/>.
/// Audit log is typically append-only.
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// when applicable.
/// </summary>
public interface IAuditLogsRepository
{
    /// <summary>Appends a new audit log entry.</summary>
    Task<Result> AppendAsync(AuditLog log, CancellationToken ct = default);

    /// <summary>
    /// Lists audit log entries for an organization within an optional time range (paged by offset/limit).
    /// </summary>
    Task<Result<IReadOnlyList<AuditLog>>> ListByOrgAsync(
        Guid orgId,
        DateTime? from,
        DateTime? to,
        int offset,
        int limit,
        CancellationToken ct = default);

    /// <summary>
    /// Lists audit log entries for a project within an optional time range (paged by offset/limit).
    /// </summary>
    Task<Result<IReadOnlyList<AuditLog>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        DateTime? from,
        DateTime? to,
        int offset,
        int limit,
        CancellationToken ct = default);
}