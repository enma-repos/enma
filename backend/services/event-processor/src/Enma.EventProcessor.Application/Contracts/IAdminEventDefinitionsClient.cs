using FluentResults;

namespace Enma.EventProcessor.Application.Contracts;

public interface IAdminEventDefinitionsClient
{
    Task<Result<IReadOnlyList<string>>> ListNamesByProjectAsync(
        Guid organizationId, Guid projectId, CancellationToken ct = default);
}
