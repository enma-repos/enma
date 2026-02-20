using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.ApiKeys;

internal static class ApiKeyDtoMapper
{
    internal static ApiKeyDto ToDto(this ApiKey model)
        => new(
            Id: model.Id,
            SdkClientId: model.SdkClientId,
            KeyPrefix: model.KeyPrefix,
            SentEventsCount: model.SentEventsCount,
            CreatedAt: model.CreatedAt,
            LastUsedAt: model.LastUsedAt,
            RevokedAt: model.RevokedAt);
    
    internal static ApiKeyFirstCreationDto ToDto(this ApiKey model, string key)
        => new(
            Id: model.Id,
            SdkClientId: model.SdkClientId,
            KeyPrefix: model.KeyPrefix,
            Key: key,
            SentEventsCount: model.SentEventsCount,
            CreatedAt: model.CreatedAt,
            LastUsedAt: model.LastUsedAt,
            RevokedAt: model.RevokedAt);
}

