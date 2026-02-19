using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class ApiKeysService : IApiKeysService
{
    private readonly IApiKeysRepository _apiKeysRepository;

    public ApiKeysService(IApiKeysRepository apiKeysRepository)
    {
        _apiKeysRepository = apiKeysRepository;
    }
}