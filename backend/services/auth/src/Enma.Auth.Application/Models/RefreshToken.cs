using Enma.Common.Errors;
using FluentResults;

namespace Enma.Auth.Application.Models;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }

    public string TokenHash { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public DateTime LastUsedAt { get; private set; }

    private RefreshToken() { }

    private RefreshToken(
        Guid id,
        Guid accountId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt,
        DateTime lastUsedAt)
    {
        Id = id;
        AccountId = accountId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        LastUsedAt = lastUsedAt;
    }

    public static Result<RefreshToken> Create(
        Guid id,
        Guid accountId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt,
        DateTime lastUsedAt)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
        {
            return Result.Fail<RefreshToken>(ApplicationErrors.Required(nameof(tokenHash)));
        }

        if (accountId == Guid.Empty)
        {
            return Result.Fail<RefreshToken>(ApplicationErrors.Required(nameof(accountId)));
        }

        return Result.Ok(new RefreshToken(
            id,
            accountId,
            tokenHash,
            createdAt,
            expiresAt,
            lastUsedAt));
    }

    public static RefreshToken Rehydrate(
        Guid id,
        Guid accountId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt,
        DateTime lastUsedAt)
    {
        return new RefreshToken(
            id,
            accountId,
            tokenHash,
            createdAt,
            expiresAt,
            lastUsedAt);
    }
}