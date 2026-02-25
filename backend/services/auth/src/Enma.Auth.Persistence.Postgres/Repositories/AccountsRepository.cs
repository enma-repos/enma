using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Connection;
using Enma.Auth.Persistence.Postgres.Entities;
using Enma.Auth.Persistence.Postgres.Mappers;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Auth.Persistence.Postgres.Repositories;

internal sealed class AccountsRepository : IAccountsRepository
{
    private readonly PostgresDbContext _context;

    public AccountsRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Account>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await _context.Accounts
            .AsNoTracking()
            .Where(x => x.Id == id && x.DeletedAt == null)
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Account>(ApplicationErrors.EntityNotFound("Account", $"id={id}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<Account>> GetByEmailAsync(string email, CancellationToken ct)
    {
        var entity = await _context.Accounts
            .AsNoTracking()
            .Where(x => x.Email == email && x.DeletedAt == null)
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Account>(ApplicationErrors.EntityNotFound("Account", $"email={email}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<bool>> ExistsAsync(string email, CancellationToken ct)
    {
        var exists = await _context.Accounts
            .AsNoTracking()
            .AnyAsync(x => x.Email == email && x.DeletedAt == null, ct);

        return Result.Ok(exists);
    }

    public async Task<Result<Guid>> CreateAsync(Account account, CancellationToken ct)
    {
        var exists = await _context.Accounts
            .AsNoTracking()
            .AnyAsync(x => x.Email == account.Email && x.DeletedAt == null, ct);

        if (exists)
        {
            return Result.Fail<Guid>(ApplicationErrors.AlreadyExists("Account", $"email={account.Email}"));
        }

        _context.Accounts.Add(account.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok(account.Id);
    }

    public async Task<Result> UpdateLastLoginAsync(Guid id, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Accounts
            .Where(x => x.Id == id && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.LastLoginAt, now)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Account", $"id={id}"))
            : Result.Ok();
    }

    public async Task<Result> UpdateStatusAsync(Guid id, AccountStatus status, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.Accounts
            .Where(x => x.Id == id && x.DeletedAt == null)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Status, status)
                    .SetProperty(x => x.UpdatedAt, now),
                ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("Account", $"id={id}"))
            : Result.Ok();
    }
}
