namespace Enma.Admin.Application.Dto.ApiKeys;

public sealed record ApiKeyFirstCreationDto(
    Guid Id,
    Guid SdkClientId,
    string KeyPrefix,
    string Key,
    long SentEventsCount,
    DateTime CreatedAt,
    DateTime? LastUsedAt,
    DateTime? RevokedAt);