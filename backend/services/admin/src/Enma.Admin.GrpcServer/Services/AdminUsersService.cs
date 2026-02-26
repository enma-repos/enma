using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.Users;
using Enma.Common.Errors.Types;
using Enma.Grpc.Admin.Users.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Enma.Admin.GrpcServer.Services;

public sealed class AdminUsersService : Enma.Grpc.Admin.Users.V1.AdminUsersService.AdminUsersServiceBase
{
    private readonly IUsersService _usersService;

    public AdminUsersService(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        if (!TryParseAccountId(request.AccountId, out var accountId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid account_id."));
        }

        var dto = new CreateUserDto(
            AccountId: accountId,
            DisplayName: request.DisplayName,
            AvatarUrl: request.HasAvatarUrl ? request.AvatarUrl : null,
            Locale: request.HasLocale ? request.Locale : null,
            Timezone: request.HasTimezone ? request.Timezone : null);

        var res = await _usersService.GetOrCreateAsync(dto, context.CancellationToken);
        if (res.IsFailed)
        {
            throw ToRpcException(res.Errors);
        }

        return new CreateUserResponse
        {
            User = ToProto(res.Value)
        };
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        if (!TryParseAccountId(request.AccountId, out var accountId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid account_id."));
        }

        var res = await _usersService.GetByIdAsync(accountId, context.CancellationToken);
        if (res.IsFailed)
        {
            throw ToRpcException(res.Errors);
        }

        return new GetUserResponse
        {
            User = ToProto(res.Value)
        };
    }

    public override async Task<ExistsUserResponse> ExistsUser(ExistsUserRequest request, ServerCallContext context)
    {
        if (!TryParseAccountId(request.AccountId, out var accountId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid account_id."));
        }

        var res = await _usersService.ExistsAsync(accountId, context.CancellationToken);
        if (res.IsFailed)
        {
            throw ToRpcException(res.Errors);
        }

        return new ExistsUserResponse { Exists = res.Value };
    }

    private static bool TryParseAccountId(string accountId, out Guid result)
        => Guid.TryParse(accountId, out result) && result != Guid.Empty;

    private static User ToProto(UserDto dto)
    {
        var user = new User
        {
            AccountId = dto.Id.ToString(),
            DisplayName = dto.DisplayName,
            CreatedAt = ToTimestamp(dto.CreatedAt),
            UpdatedAt = ToTimestamp(dto.UpdatedAt)
        };

        if (!string.IsNullOrWhiteSpace(dto.AvatarUrl))
        {
            user.AvatarUrl = dto.AvatarUrl;
        }

        if (!string.IsNullOrWhiteSpace(dto.Locale))
        {
            user.Locale = dto.Locale;
        }

        if (!string.IsNullOrWhiteSpace(dto.Timezone))
        {
            user.Timezone = dto.Timezone;
        }

        if (dto.DeletedAt is not null)
        {
            user.DeletedAt = ToTimestamp(dto.DeletedAt.Value);
        }

        return user;
    }

    private static Timestamp ToTimestamp(DateTime value)
    {
        var utc = value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => value.ToUniversalTime()
        };

        return Timestamp.FromDateTime(utc);
    }

    private static RpcException ToRpcException(IReadOnlyList<FluentResults.IError> errors)
    {
        var err = errors.FirstOrDefault();
        if (err is null)
        {
            return new RpcException(new Status(StatusCode.Internal, "Unknown error."));
        }

        return err switch
        {
            ValidationError => new RpcException(new Status(StatusCode.InvalidArgument, err.Message)),
            NotFoundError => new RpcException(new Status(StatusCode.NotFound, err.Message)),
            ConflictError => new RpcException(new Status(StatusCode.AlreadyExists, err.Message)),
            ForbiddenError => new RpcException(new Status(StatusCode.PermissionDenied, err.Message)),
            _ => new RpcException(new Status(StatusCode.Internal, err.Message))
        };
    }
}
