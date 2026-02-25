using Enma.Common.Errors;
using FluentResults;

namespace Enma.Auth.Application.Models;

public sealed class ExternalAuth
{
    public string Provider { get; private set; } = null!;
    public string Subject { get; private set; } = null!;

    public Guid AccountId { get; private set; }

    public DateTime LinkedAt { get; private set; }
    public DateTime LastLoginAt { get; private set; }

    private ExternalAuth() { }

    private ExternalAuth(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt,
        DateTime lastLoginAt)
    {
        Provider = provider;
        Subject = subject;
        AccountId = accountId;
        LinkedAt = linkedAt;
        LastLoginAt = lastLoginAt;
    }

    public static Result<ExternalAuth> Create(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt,
        DateTime lastLoginAt)
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            return Result.Fail<ExternalAuth>(ApplicationErrors.Required(nameof(provider)));
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            return Result.Fail<ExternalAuth>(ApplicationErrors.Required(nameof(subject)));
        }

        if (accountId == Guid.Empty)
        {
            return Result.Fail<ExternalAuth>(ApplicationErrors.Required(nameof(accountId)));
        }

        return Result.Ok(new ExternalAuth(
            provider,
            subject,
            accountId,
            linkedAt,
            lastLoginAt));
    }

    public static ExternalAuth Rehydrate(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt,
        DateTime lastLoginAt)
    {
        return new ExternalAuth(
            provider,
            subject,
            accountId,
            linkedAt,
            lastLoginAt);
    }
}