using Enma.EventProcessor.Application.Contracts;
using Enma.EventProcessor.Infrastructure.Grpc.Admin.Options;
using Enma.EventProcessor.Infrastructure.Grpc.Admin.Utils;
using Enma.Grpc.Admin.EventDefinitions.V1;
using FluentResults;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Enma.EventProcessor.Infrastructure.Grpc.Admin.Clients;

internal sealed class AdminEventDefinitionsClient : IAdminEventDefinitionsClient
{
    private readonly AdminEventDefinitionsService.AdminEventDefinitionsServiceClient _client;
    private readonly AdminGrpcOptions _options;

    public AdminEventDefinitionsClient(
        AdminEventDefinitionsService.AdminEventDefinitionsServiceClient client,
        IOptions<AdminGrpcOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<Result<IReadOnlyList<string>>> ListNamesByProjectAsync(
        Guid organizationId, Guid projectId, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.ListNamesByProjectAsync(
                    new ListNamesByProjectRequest
                    {
                        OrganizationId = organizationId.ToString(),
                        ProjectId = projectId.ToString()
                    },
                    deadline: AdminGrpcUtils.GetDeadline(_options.DeadlineMs),
                    cancellationToken: ct)
                .ResponseAsync;

            return Result.Ok<IReadOnlyList<string>>(response.Names.ToList());
        }
        catch (RpcException ex)
        {
            return Result.Fail<IReadOnlyList<string>>(
                AdminGrpcUtils.Map(ex, "EventDefinitions", $"orgId={organizationId}, projectId={projectId}"));
        }
    }
}
