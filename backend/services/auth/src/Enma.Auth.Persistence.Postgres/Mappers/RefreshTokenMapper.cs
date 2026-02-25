using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Entities;

namespace Enma.Auth.Persistence.Postgres.Mappers;

internal static class RefreshTokenMapper
{
    internal static RefreshToken ToModel(this RefreshTokenEntity entity)
        => RefreshToken.Rehydrate(
            id: entity.Id,
            accountId: entity.AccountId,
            tokenHash: entity.TokenHash,
            createdAt: entity.CreatedAt,
            expiresAt: entity.ExpiresAt,
            lastUsedAt: entity.LastUsedAt);

    internal static RefreshTokenEntity ToEntity(this RefreshToken model)
        => new()
        {
            Id = model.Id,
            AccountId = model.AccountId,
            Account = null!,
            TokenHash = model.TokenHash,
            CreatedAt = model.CreatedAt,
            ExpiresAt = model.ExpiresAt,
            LastUsedAt = model.LastUsedAt
        };
}

