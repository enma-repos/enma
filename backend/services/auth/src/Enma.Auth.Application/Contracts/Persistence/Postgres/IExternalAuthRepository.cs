using Enma.Auth.Application.Models;
using FluentResults;

namespace Enma.Auth.Application.Contracts.Persistence.Postgres;

public interface IExternalAuthRepository
{
    Task<Result<ExternalAuth>> GetAsync(string provider, string subject, CancellationToken ct);
    Task<Result<Account>> GetAccountByExternalAsync(string provider, string subject, CancellationToken ct);
    Task<Result> CreateAsync(ExternalAuth externalAuth, CancellationToken ct);
    Task<Result> UpdateLastLoginAsync(string provider, string subject, CancellationToken ct);
}