using System.Text.Json.Nodes;
using Enma.Admin.Application.Models;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="SdkClient"/> (formerly ApiClient).
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// when the entity does not exist.
/// </summary>
public interface ISdkClientsRepository
{
    /// <summary>Creates a new SDK client.</summary>
    Task<Result<SdkClient>> CreateAsync(SdkClient client, Guid orgId, CancellationToken ct = default);

    /// <summary>Gets an SDK client by id.</summary>
    Task<Result<SdkClient>> GetByIdAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Lists SDK clients for a project (paged by page/pageSize).</summary>
    Task<Result<IReadOnlyList<SdkClient>>> ListByProjectAsync(Guid projectId, Guid orgId, int page, int pageSize, string? search = null, CancellationToken ct = default);

    /// <summary>Counts SDK clients for a project.</summary>
    Task<Result<int>> CountByProjectAsync(Guid projectId, Guid orgId, string? search = null, CancellationToken ct = default);

    /// <summary>Persists a full SDK client update (when a fully loaded domain model is available).</summary>
    Task<Result> UpdateAsync(SdkClient client, CancellationToken ct = default);

    /// <summary>Updates <see cref="SdkClient.Name"/> only.</summary>
    Task<Result> SetNameAsync(Guid clientId, Guid projectId, Guid orgId, string name, CancellationToken ct = default);

    /// <summary>Updates <see cref="SdkClient.Description"/> only.</summary>
    Task<Result> SetDescriptionAsync(Guid clientId, Guid projectId, Guid orgId, string? description, CancellationToken ct = default);

    /// <summary>
    /// Updates <see cref="SdkClient.Settings"/> only. Pass null to clear the value.
    /// Stored as jsonb on persistence side.
    /// </summary>
    Task<Result> SetSettingsAsync(Guid clientId, Guid projectId, Guid orgId, JsonObject? settings, CancellationToken ct = default);

    /// <summary>Updates <see cref="SdkClient.Type"/> only.</summary>
    Task<Result> SetTypeAsync(Guid clientId, Guid projectId, Guid orgId, SdkClientType type, CancellationToken ct = default);

    /// <summary>Disables an SDK client (sets DisabledAt).</summary>
    Task<Result> SetDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Enables an SDK client (clears DisabledAt).</summary>
    Task<Result> ClearDisabledAsync(Guid clientId, Guid projectId, Guid orgId, CancellationToken ct = default);
}
