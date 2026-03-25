using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Common.Errors;
using Enma.Grpc.Admin.Users.V1;
using FluentResults;
using Google.Protobuf.WellKnownTypes;

namespace Enma.Auth.Infrastructure.Grpc.Admin.Mapping;

internal static class AdminUsersMapper
{
    internal static CreateUserRequest ToProto(this CreateAdminUserDto dto)
    {
        var req = new CreateUserRequest
        {
            AccountId = dto.AccountId.ToString(),
            Email = dto.Email,
            DisplayName = dto.DisplayName
        };

        if (!string.IsNullOrWhiteSpace(dto.AvatarUrl))
        {
            req.AvatarUrl = dto.AvatarUrl;
        }

        if (!string.IsNullOrWhiteSpace(dto.Locale))
        {
            req.Locale = dto.Locale;
        }

        if (!string.IsNullOrWhiteSpace(dto.Timezone))
        {
            req.Timezone = dto.Timezone;
        }

        return req;
    }

    internal static Result<AdminUserDto> ToDtoResult(this User user)
    {
        if (!Guid.TryParse(user.AccountId, out var accountId) || accountId == Guid.Empty)
        {
            return Result.Fail<AdminUserDto>(ApplicationErrors.InvariantViolation("Admin returned invalid user.account_id."));
        }

        if (user.CreatedAt is null || user.UpdatedAt is null)
        {
            return Result.Fail<AdminUserDto>(ApplicationErrors.InvariantViolation("Admin returned user with missing timestamps."));
        }

        return Result.Ok(new AdminUserDto(
            AccountId: accountId,
            DisplayName: user.DisplayName,
            AvatarUrl: user.HasAvatarUrl ? user.AvatarUrl : null,
            Locale: user.HasLocale ? user.Locale : null,
            Timezone: user.HasTimezone ? user.Timezone : null,
            CreatedAt: FromTimestamp(user.CreatedAt),
            UpdatedAt: FromTimestamp(user.UpdatedAt),
            DeletedAt: user.DeletedAt is null ? null : FromTimestamp(user.DeletedAt)));
    }

    private static DateTime FromTimestamp(Timestamp ts)
    {
        var value = ts.ToDateTime();
        return value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
