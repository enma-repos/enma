using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="User"/>.
/// User.Id is expected to be equal to AccountId from the auth service (1:1 mapping).
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// (e.g. <c>NotFoundError</c>) when the entity does not exist.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Creates a new user profile record if it doesn't exist, otherwise returns the existing one.
    /// </summary>
    Task<Result<User>> GetOrCreateAsync(User user, CancellationToken ct = default);

    /// <summary>Gets a user by id (equals auth AccountId).</summary>
    Task<Result<User>> GetByIdAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Gets a user by email address.</summary>
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>Checks whether a user exists (excluding soft-deleted records).</summary>
    Task<Result<bool>> ExistsAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Persists a full user update (when a fully loaded domain model is available).</summary>
    Task<Result> UpdateAsync(User user, CancellationToken ct = default);

    /// <summary>Updates <see cref="User.DisplayName"/> only.</summary>
    Task<Result> SetDisplayNameAsync(Guid userId, string displayName, CancellationToken ct = default);

    /// <summary>Updates <see cref="User.AvatarUrl"/> only. Pass null to clear the value.</summary>
    Task<Result> SetAvatarUrlAsync(Guid userId, string? avatarUrl, CancellationToken ct = default);

    /// <summary>Updates <see cref="User.Locale"/> only. Pass null to clear the value.</summary>
    Task<Result> SetLocaleAsync(Guid userId, string? locale, CancellationToken ct = default);

    /// <summary>Updates <see cref="User.Timezone"/> only. Pass null to clear the value.</summary>
    Task<Result> SetTimezoneAsync(Guid userId, string? timezone, CancellationToken ct = default);

    /// <summary>Soft-deletes a user (sets DeletedAt).</summary>
    Task<Result> SoftDeleteAsync(Guid userId, CancellationToken ct = default);
}
