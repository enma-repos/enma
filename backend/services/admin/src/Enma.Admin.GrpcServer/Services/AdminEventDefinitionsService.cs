using Enma.Admin.Application.Contracts;
using Enma.Common.Errors.Types;
using Enma.Grpc.Admin.EventDefinitions.V1;
using Grpc.Core;

namespace Enma.Admin.GrpcServer.Services;

public sealed class AdminEventDefinitionsService
    : Enma.Grpc.Admin.EventDefinitions.V1.AdminEventDefinitionsService.AdminEventDefinitionsServiceBase
{
    private readonly IEventDefinitionsRepository _repository;

    public AdminEventDefinitionsService(IEventDefinitionsRepository repository)
    {
        _repository = repository;
    }

    public override async Task<ListNamesByProjectResponse> ListNamesByProject(
        ListNamesByProjectRequest request, ServerCallContext context)
    {
        if (!TryParseGuid(request.OrganizationId, out var orgId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid organization_id."));
        }

        if (!TryParseGuid(request.ProjectId, out var projectId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid project_id."));
        }

        var res = await _repository.ListNamesByProjectAsync(projectId, orgId, context.CancellationToken);
        if (res.IsFailed)
        {
            throw ToRpcException(res.Errors);
        }

        var response = new ListNamesByProjectResponse();
        response.Names.AddRange(res.Value);
        return response;
    }

    private static bool TryParseGuid(string value, out Guid result)
        => Guid.TryParse(value, out result) && result != Guid.Empty;

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
