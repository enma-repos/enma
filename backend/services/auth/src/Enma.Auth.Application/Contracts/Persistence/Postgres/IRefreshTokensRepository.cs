using Enma.Auth.Application.Models;
using FluentResults;

namespace Enma.Auth.Application.Contracts.Persistence.Postgres;

public interface IRefreshTokensRepository
{
    Task<Result<RefreshToken>> GetByTokenHashAsync(string tokenHash, CancellationToken ct);
    Task<Result<List<RefreshToken>>> GetByAccountIdAsync(Guid accountId, CancellationToken ct); 
    Task<Result> UpdateLastLoginAsync(Guid tokenId, CancellationToken ct);
}