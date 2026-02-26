using Enma.Common.Errors;
using FluentResults;

namespace Enma.Auth.Application.Models;

public sealed class ExternalAuth
{
    public string Provider { get; private set; } = null!;
    public string Subject { get; private set; } = null!;

    public Guid AccountId { get; private set; }

    public DateTime LinkedAt { get; private set; }

    private ExternalAuth() { }

    private ExternalAuth(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt)
    {
        Provider = provider;
        Subject = subject;
        AccountId = accountId;
        LinkedAt = linkedAt;
    }

    public static Result<ExternalAuth> Create(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt)
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
            linkedAt));
    }

    public static ExternalAuth Rehydrate(
        string provider,
        string subject,
        Guid accountId,
        DateTime linkedAt)
    {
        return new ExternalAuth(
            provider,
            subject,
            accountId,
            linkedAt);
    }
}