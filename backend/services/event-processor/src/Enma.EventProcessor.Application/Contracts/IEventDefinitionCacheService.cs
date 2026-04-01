namespace Enma.EventProcessor.Application.Contracts;

public interface IEventDefinitionCacheService
{
    Task<HashSet<string>?> GetAllowedNamesAsync(Guid orgId, Guid projectId, CancellationToken ct = default);
    Task SetAllowedNamesAsync(Guid orgId, Guid projectId, IReadOnlyList<string> names, CancellationToken ct = default);
    Task InvalidateAsync(Guid orgId, Guid projectId, CancellationToken ct = default);
}
