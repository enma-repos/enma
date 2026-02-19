using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class SdkClientsService : ISdkClientsService
{
    private readonly ISdkClientsRepository _sdkClientsRepository;

    public SdkClientsService(ISdkClientsRepository sdkClientsRepository)
    {
        _sdkClientsRepository = sdkClientsRepository;
    }
}