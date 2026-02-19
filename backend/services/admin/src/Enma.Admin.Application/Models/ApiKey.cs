using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class ApiKey
{
    public Guid Id { get; private set; }
    public Guid SdkClientId { get; private set; }

    public string KeyPrefix { get; private set; } = null!;
    public string KeyHash { get; private set; } = null!;

    public long SentEventsCount { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUsedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    
    private ApiKey() { }
    
    private ApiKey(Guid id, Guid sdkClientId, string keyPrefix, string keyHash, DateTime createdAt)
    {
        Id = id;
        SdkClientId = sdkClientId;
        KeyPrefix = keyPrefix;
        KeyHash = keyHash;
        CreatedAt = createdAt;
    }
    
    public static Result<ApiKey> Create(Guid id, Guid sdkClientId, string? keyPrefix, string? keyHash, 
        DateTime createdAt)
    {
        if (sdkClientId == Guid.Empty)
        {
            return Result.Fail<ApiKey>(ApplicationErrors.Required(nameof(sdkClientId)));
        }

        keyPrefix = (keyPrefix ?? string.Empty).Trim();
        if (keyPrefix.Length is < 6 or > 32)
        {
            return Result.Fail<ApiKey>(ApplicationErrors.Length(nameof(keyPrefix), 6, 32));
        }

        if (string.IsNullOrWhiteSpace(keyHash))
        {
            return Result.Fail<ApiKey>(ApplicationErrors.Required(nameof(keyHash)));
        }

        return Result.Ok(new ApiKey(id, sdkClientId, keyPrefix, keyHash, createdAt));
    }
    
    public static ApiKey Rehydrate(Guid id, Guid sdkClientId, string keyPrefix, string keyHash, long sentEventsCount,
        DateTime createdAt, DateTime? lastUsedAt, DateTime? revokedAt)
    {
        return new ApiKey
        {
            Id = id,
            SdkClientId = sdkClientId,
            KeyPrefix = keyPrefix,
            KeyHash = keyHash,
            SentEventsCount = sentEventsCount,
            CreatedAt = createdAt,
            LastUsedAt = lastUsedAt,
            RevokedAt = revokedAt
        };
    }
    
    internal bool HashEquals(string providedHash) => string.Equals(KeyHash, providedHash, StringComparison.Ordinal);
}