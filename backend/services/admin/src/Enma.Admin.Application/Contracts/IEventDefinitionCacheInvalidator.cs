namespace Enma.Admin.Application.Contracts;

public interface IEventDefinitionCacheInvalidator
{
    Task InvalidateAsync(Guid orgId, Guid projectId, CancellationToken ct = default);
}
