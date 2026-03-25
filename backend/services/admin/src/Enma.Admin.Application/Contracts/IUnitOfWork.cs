namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Provides transactional scope for operations that span multiple repositories.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Executes the given action inside a database transaction.
    /// Commits on success, rolls back on failure.
    /// </summary>
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
}
