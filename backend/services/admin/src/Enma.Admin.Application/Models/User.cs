using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class User
{
    public Guid Id { get; private set; }

    public string DisplayName { get; private set; } = null!;
    
    public string? AvatarUrl { get; private set; }
    public string? Locale { get; private set; }
    public string? Timezone { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private User() { }

    private User(Guid id, string displayName, string? avatarUrl, string? locale, string? timezone, DateTime createdAt)
    {
        Id = id;
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
        Locale = locale;
        Timezone = timezone;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
    
    public static Result<User> Create(Guid accountId, string? displayName, string? avatarUrl, string? locale, 
        string? timezone, DateTime createdAt)
    {
        displayName = (displayName ?? String.Empty).Trim();
        if (displayName.Length is < 2 or > 200)
        {
            return Result.Fail<User>(ApplicationErrors.Length("DisplayName", 2, 200));
        }

        return Result.Ok(new User(accountId, displayName, avatarUrl, locale, timezone, createdAt));
    }
    
    internal static User Rehydrate(Guid id, string displayName, string? avatarUrl, string? locale, string? timezone, 
        DateTime createdAt, DateTime updatedAt, DateTime? deletedAt)
    {
        return new User
        {
            Id = id,
            DisplayName = displayName,
            AvatarUrl = avatarUrl,
            Locale = locale,
            Timezone = timezone,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
}