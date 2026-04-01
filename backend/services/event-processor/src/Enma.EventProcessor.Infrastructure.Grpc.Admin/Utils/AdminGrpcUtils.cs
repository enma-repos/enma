using Enma.Common.Errors;
using FluentResults;
using Grpc.Core;

namespace Enma.EventProcessor.Infrastructure.Grpc.Admin.Utils;

internal static class AdminGrpcUtils
{
    internal static IError Map(RpcException ex, string entity, string hint)
    {
        var detail = string.IsNullOrWhiteSpace(ex.Status.Detail) ? ex.Message : ex.Status.Detail;

        return ex.StatusCode switch
        {
            StatusCode.InvalidArgument => ApplicationErrors.Validation(detail),
            StatusCode.NotFound => ApplicationErrors.EntityNotFound(entity, hint),
            StatusCode.AlreadyExists => ApplicationErrors.AlreadyExists(entity, hint),
            StatusCode.PermissionDenied => ApplicationErrors.Forbidden(detail),
            _ => new Error(detail)
        };
    }

    internal static DateTime GetDeadline(int ms) => DateTime.UtcNow.AddMilliseconds(ms);
}
