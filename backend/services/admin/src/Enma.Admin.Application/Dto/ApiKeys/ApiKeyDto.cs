namespace Enma.Admin.Application.Dto.ApiKeys;

public sealed record ApiKeyDto(
    Guid Id,
    Guid SdkClientId,
    string KeyPrefix,
    long SentEventsCount,
    DateTime CreatedAt,
    DateTime? LastUsedAt,
    DateTime? RevokedAt);

