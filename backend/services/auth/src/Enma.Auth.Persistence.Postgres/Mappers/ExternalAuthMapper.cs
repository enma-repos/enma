using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Entities;

namespace Enma.Auth.Persistence.Postgres.Mappers;

internal static class ExternalAuthMapper
{
    internal static ExternalAuth ToModel(this ExternalAuthEntity entity)
        => ExternalAuth.Rehydrate(
            provider: entity.Provider,
            subject: entity.Subject,
            accountId: entity.AccountId,
            linkedAt: entity.LinkedAt,
            lastLoginAt: entity.LastLoginAt);

    internal static ExternalAuthEntity ToEntity(this ExternalAuth model)
        => new()
        {
            Provider = model.Provider,
            Subject = model.Subject,
            AccountId = model.AccountId,
            LinkedAt = model.LinkedAt,
            LastLoginAt = model.LastLoginAt
        };
}

