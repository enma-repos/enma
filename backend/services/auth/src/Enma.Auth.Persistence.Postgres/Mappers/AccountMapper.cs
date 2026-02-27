using Enma.Auth.Application.Models;
using Enma.Auth.Persistence.Postgres.Entities;

namespace Enma.Auth.Persistence.Postgres.Mappers;

internal static class AccountMapper
{
    internal static Account ToModel(this AccountEntity entity)
        => Account.Rehydrate(
            id: entity.Id,
            email: entity.Email,
            status: entity.Status,
            passwordHash: entity.PasswordHash,
            salt: entity.Salt,
            lastLoginAt: entity.LastLoginAt,
            onboardingStartedAt: entity.OnboardingStartedAt,
            onboardingCompletedAt: entity.OnboardingCompletedAt,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            deletedAt: entity.DeletedAt);

    internal static AccountEntity ToEntity(this Account model)
        => new()
        {
            Id = model.Id,
            Email = model.Email.Value,
            Status = model.Status,
            PasswordHash = model.PasswordHash,
            Salt = model.Salt,
            LastLoginAt = model.LastLoginAt,
            OnboardingStartedAt = model.OnboardingStartedAt,
            OnboardingCompletedAt = model.OnboardingCompletedAt,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt
        };
}

