using Enma.Auth.Application.Models;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Auth.Application.Contracts.Persistence.Postgres;

public interface IAccountsRepository
{
    Task<Result<Account>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<Account>> GetByEmailAsync(string email, CancellationToken ct);
    Task<Result<bool>> ExistsAsync(string email, CancellationToken ct);
    Task<Result<Guid>> CreateAsync(Account account, CancellationToken ct);
    Task<Result> UpdateLastLoginAsync(Guid id, CancellationToken ct);
    Task<Result> UpdateStatusAsync(Guid id, AccountStatus status, CancellationToken ct);
    Task<Result> CompleteOnboardingAsync(Guid id, DateTime completedAt, CancellationToken ct);
}
