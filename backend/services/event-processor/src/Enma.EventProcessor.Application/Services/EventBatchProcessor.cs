using Enma.Common.Models;
using Enma.EventProcessor.Application.Abstractions;
using Enma.EventProcessor.Application.Contracts;
using Enma.EventProcessor.Application.Contracts.Persistence.ClickHouse;
using Microsoft.Extensions.Logging;

namespace Enma.EventProcessor.Application.Services;

internal sealed class EventBatchProcessor : IEventBatchProcessor
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IEventDefinitionCacheService _cache;
    private readonly IAdminEventDefinitionsClient _adminClient;
    private readonly ILogger<EventBatchProcessor> _logger;

    public EventBatchProcessor(
        IEventsRepository eventsRepository,
        IEventDefinitionCacheService cache,
        IAdminEventDefinitionsClient adminClient,
        ILogger<EventBatchProcessor> logger)
    {
        _eventsRepository = eventsRepository;
        _cache = cache;
        _adminClient = adminClient;
        _logger = logger;
    }

    public async Task ProcessBatchAsync(IReadOnlyCollection<EventMessageDto> events, CancellationToken ct = default)
    {
        if (events.Count == 0)
        {
            return;
        }

        var validEvents = new List<EventMessageDto>(events.Count);
        var rejectedCount = 0;

        foreach (var group in events.GroupBy(e => (e.OrganizationId, e.ProjectId)))
        {
            var allowedNames = await GetAllowedNamesAsync(
                group.Key.OrganizationId, group.Key.ProjectId, ct);

            foreach (var evt in group)
            {
                if (allowedNames.Contains(evt.EventName))
                {
                    validEvents.Add(evt);
                }
                else
                {
                    rejectedCount++;
                    _logger.LogWarning(
                        "Rejected unregistered event '{EventName}' for project {ProjectId}.",
                        evt.EventName, group.Key.ProjectId);
                }
            }
        }

        if (rejectedCount > 0)
        {
            _logger.LogWarning("Rejected {Count} unregistered events in batch.", rejectedCount);
        }

        if (validEvents.Count > 0)
        {
            _logger.LogInformation("Processing batch of {Count} valid events.", validEvents.Count);
            await _eventsRepository.InsertBatchAsync(validEvents, ct);
            _logger.LogInformation("Inserted batch of {Count} events into ClickHouse.", validEvents.Count);
        }
    }

    private async Task<HashSet<string>> GetAllowedNamesAsync(
        Guid orgId, Guid projectId, CancellationToken ct)
    {
        var cached = await _cache.GetAllowedNamesAsync(orgId, projectId, ct);
        if (cached is not null)
        {
            return cached;
        }

        var result = await _adminClient.ListNamesByProjectAsync(orgId, projectId, ct);
        if (result.IsFailed)
        {
            _logger.LogError(
                "Failed to fetch event definitions for project {ProjectId}. Will retry via redelivery.",
                projectId);
            throw new InvalidOperationException(
                $"Admin service unavailable for project {projectId}");
        }

        var names = result.Value;
        var set = new HashSet<string>(names, StringComparer.Ordinal);

        await _cache.SetAllowedNamesAsync(orgId, projectId, names, ct);

        return set;
    }
}
