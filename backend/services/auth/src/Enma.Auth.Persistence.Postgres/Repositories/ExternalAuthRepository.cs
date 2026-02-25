using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Connection;
using Enma.Auth.Persistence.Postgres.Entities;
using Enma.Auth.Persistence.Postgres.Mappers;
using Enma.Common.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Enma.Auth.Persistence.Postgres.Repositories;

internal sealed class ExternalAuthRepository : IExternalAuthRepository
{
    private readonly PostgresDbContext _context;

    public ExternalAuthRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ExternalAuth>> GetAsync(string provider, string subject, CancellationToken ct)
    {
        var entity = await _context.ExternalAuth
            .AsNoTracking()
            .Where(x => x.Provider == provider && x.Subject == subject)
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<ExternalAuth>(ApplicationErrors.EntityNotFound("ExternalAuth", $"provider={provider}, subject={subject}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result<Account>> GetAccountByExternalAsync(string provider, string subject, CancellationToken ct)
    {
        var entity = await _context.ExternalAuth
            .AsNoTracking()
            .Where(x => x.Provider == provider && x.Subject == subject && x.Account.DeletedAt == null)
            .Select(x => new AccountEntity
            {
                Id = x.Account.Id,
                Email = x.Account.Email,
                Status = x.Account.Status,
                PasswordHash = x.Account.PasswordHash,
                Salt = x.Account.Salt,
                LastLoginAt = x.Account.LastLoginAt,
                OnboardingStartedAt = x.Account.OnboardingStartedAt,
                OnboardingCompletedAt = x.Account.OnboardingCompletedAt,
                CreatedAt = x.Account.CreatedAt,
                UpdatedAt = x.Account.UpdatedAt,
                DeletedAt = x.Account.DeletedAt
            })
            .FirstOrDefaultAsync(ct);

        return entity is null
            ? Result.Fail<Account>(ApplicationErrors.EntityNotFound("Account", $"provider={provider}, subject={subject}"))
            : Result.Ok(entity.ToModel());
    }

    public async Task<Result> CreateAsync(ExternalAuth externalAuth, CancellationToken ct)
    {
        var exists = await _context.ExternalAuth
            .AsNoTracking()
            .AnyAsync(x => x.Provider == externalAuth.Provider && x.Subject == externalAuth.Subject, ct);

        if (exists)
        {
            return Result.Fail(ApplicationErrors.AlreadyExists(
                "ExternalAuth",
                $"provider={externalAuth.Provider}, subject={externalAuth.Subject}"));
        }

        _context.ExternalAuth.Add(externalAuth.ToEntity());
        await _context.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result> UpdateLastLoginAsync(string provider, string subject, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var affected = await _context.ExternalAuth
            .Where(x => x.Provider == provider && x.Subject == subject)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.LastLoginAt, now), ct);

        return affected == 0
            ? Result.Fail(ApplicationErrors.EntityNotFound("ExternalAuth", $"provider={provider}, subject={subject}"))
            : Result.Ok();
    }
}
