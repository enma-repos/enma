using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class ApiKeyMapper
{
    internal static ApiKey ToModel(this ApiKeyEntity entity)
        => ApiKey.Rehydrate(
            id: entity.Id,
            sdkClientId: entity.SdkClientId,
            keyPrefix: entity.KeyPrefix,
            keyHash: entity.KeyHash,
            sentEventsCount: entity.SentEventsCount,
            createdAt: entity.CreatedAt,
            lastUsedAt: entity.LastUsedAt,
            revokedAt: entity.RevokedAt);

    internal static ApiKeyEntity ToEntity(this ApiKey model)
        => new()
        {
            Id = model.Id,
            SdkClientId = model.SdkClientId,
            KeyPrefix = model.KeyPrefix,
            KeyHash = model.KeyHash,
            SentEventsCount = model.SentEventsCount,
            CreatedAt = model.CreatedAt,
            LastUsedAt = model.LastUsedAt,
            RevokedAt = model.RevokedAt
        };
}

