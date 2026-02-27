using Enma.Auth.Application.ValueObjects;
using Enma.Common.Constants;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Auth.Application.Models;

public sealed class Account
{
    public Guid Id { get; private set; }

    public EmailAddress Email { get; private set; }
    public AccountStatus Status { get; private set; }

    public string? PasswordHash { get; private set; }
    public string? Salt { get; private set; }

    public DateTime LastLoginAt { get; private set; }

    public DateTime OnboardingStartedAt { get; private set; }
    public DateTime? OnboardingCompletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Account() { }
    
    private Account(
        Guid id,
        EmailAddress email,
        AccountStatus status,
        string? passwordHash,
        string? salt,
        DateTime lastLoginAt,
        DateTime onboardingStartedAt,
        DateTime? onboardingCompletedAt,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? deletedAt)
    {
        Id = id;
        Email = email;
        Status = status;
        PasswordHash = passwordHash;
        Salt = salt;
        LastLoginAt = lastLoginAt;
        OnboardingStartedAt = onboardingStartedAt;
        OnboardingCompletedAt = onboardingCompletedAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public static Result<Account> Create(
        Guid id,
        string email,
        AccountStatus status,
        string? passwordHash,
        string? salt,
        DateTime lastLoginAt,
        DateTime onboardingStartedAt,
        DateTime? onboardingCompletedAt,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? deletedAt)
    {
        if (string.IsNullOrWhiteSpace(email) || !RegexPatterns.Email().IsMatch(email))
        {
            return Result.Fail<Account>(ApplicationErrors.Validation("Invalid email."));
        }

        var emailResult = EmailAddress.Create(email);
        if (emailResult.IsFailed)
        {
            return Result.Fail<Account>(emailResult.Errors);
        }
        
        return Result.Ok(new Account(
            id,
            emailResult.Value,
            status,
            passwordHash,
            salt,
            lastLoginAt,
            onboardingStartedAt,
            onboardingCompletedAt,
            createdAt,
            updatedAt,
            deletedAt));
    }

    public static Account Rehydrate(
        Guid id,
        string email,
        AccountStatus status,
        string? passwordHash,
        string? salt,
        DateTime lastLoginAt,
        DateTime onboardingStartedAt,
        DateTime? onboardingCompletedAt,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? deletedAt)
    {
        return new Account(
            id,
            EmailAddress.Create(email).Value,
            status,
            passwordHash,
            salt,
            lastLoginAt,
            onboardingStartedAt,
            onboardingCompletedAt,
            createdAt,
            updatedAt,
            deletedAt);
    }
}