using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Services;

internal sealed class ChainStitchingService : IChainStitchingService
{
    public Result<ChainAggregationSlice> BuildSlice(
        BucketWindow? window,
        ChainCursor? existingCursor,
        IReadOnlyList<PathSourceEvent>? orderedEvents)
    {
        var errors = new List<IError>();

        if (window is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(window)));
        }

        if (orderedEvents is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(orderedEvents)));
        }

        if (errors.Count > 0)
        {
            return Result.Fail<ChainAggregationSlice>(errors);
        }

        if (orderedEvents!.Count == 0)
        {
            return Result.Ok(ChainAggregationSlice.Empty);
        }

        var chainKey = orderedEvents[0].ChainKey;

        if (existingCursor is not null && existingCursor.ChainKey != chainKey)
        {
            errors.Add(ApplicationErrors.Validation("existingCursor chain key must match ordered events chain key."));
        }

        for (var i = 0; i < orderedEvents.Count; i++)
        {
            var current = orderedEvents[i];

            if (current.ChainKey != chainKey)
            {
                errors.Add(ApplicationErrors.Validation("All ordered events must belong to the same chain."));
            }

            if (current.OccurredAtUtc < window!.StartUtc.Value || current.OccurredAtUtc >= window.EndUtc.Value)
            {
                errors.Add(ApplicationErrors.Validation("Ordered events must belong to the requested bucket window."));
            }

            if (i > 0 && current.OccurredAtUtc < orderedEvents[i - 1].OccurredAtUtc)
            {
                errors.Add(ApplicationErrors.Validation("orderedEvents must be sorted ascending by occurrence time."));
            }
        }

        if (errors.Count > 0)
        {
            return Result.Fail<ChainAggregationSlice>(errors);
        }

        // Determine the entry event name for this chain.
        // If no existing cursor, this is a new chain — the first event is the entry.
        // If a cursor exists, carry forward its FirstEventName.
        var entryEventName = existingCursor is null
            ? orderedEvents[0].EventName
            : existingCursor.FirstEventName;

        var nodeVisits = new List<NodeVisit>(orderedEvents.Count);
        var edgeTransitions = new List<EdgeTransition>(Math.Max(orderedEvents.Count, 1));

        for (var i = 0; i < orderedEvents.Count; i++)
        {
            var current = orderedEvents[i];

            var nodeKeyResult = NodeKey.Create(
                window!.StartUtc.Value,
                chainKey.OrganizationId,
                chainKey.ProjectId,
                chainKey.ProcessDefinitionId,
                current.EventName.Value);

            if (nodeKeyResult.IsFailed)
            {
                errors.AddRange(nodeKeyResult.Errors);
            }
            else
            {
                nodeVisits.Add(new NodeVisit(
                    chainKey,
                    nodeKeyResult.Value,
                    IsEntry: i == 0 && existingCursor is null,
                    IsExit: i == orderedEvents.Count - 1,
                    EntryEventName: entryEventName));
            }

            if (i == 0 && existingCursor is not null)
            {
                var boundaryEdgeResult = CreateEdgeTransition(window, chainKey, existingCursor.LastEventName, current, entryEventName);
                if (boundaryEdgeResult.IsFailed)
                {
                    errors.AddRange(boundaryEdgeResult.Errors);
                }
                else
                {
                    edgeTransitions.Add(boundaryEdgeResult.Value);
                }
            }

            if (i > 0)
            {
                var edgeResult = CreateEdgeTransition(window, chainKey, orderedEvents[i - 1].EventName, current, entryEventName);
                if (edgeResult.IsFailed)
                {
                    errors.AddRange(edgeResult.Errors);
                }
                else
                {
                    edgeTransitions.Add(edgeResult.Value);
                }
            }
        }

        var lastEvent = orderedEvents[^1];

        var cursorResult = ChainCursor.Create(
            chainKey,
            lastEvent.EventId,
            lastEvent.EventName.Value,
            lastEvent.OccurredAtUtc,
            lastEvent.ActorUserId?.Value,
            lastEvent.ActorAnonymousId?.Value,
            window!.EndUtc.Value,
            entryEventName.Value);

        if (cursorResult.IsFailed)
        {
            errors.AddRange(cursorResult.Errors);
        }

        if (errors.Count > 0)
        {
            return Result.Fail<ChainAggregationSlice>(errors);
        }

        return Result.Ok(new ChainAggregationSlice(nodeVisits, edgeTransitions, cursorResult.Value));
    }

    private static Result<EdgeTransition> CreateEdgeTransition(
        BucketWindow window,
        ChainKey chainKey,
        EventName fromEvent,
        PathSourceEvent toEvent,
        EventName entryEventName)
    {
        var edgeKeyResult = EdgeKey.Create(
            window.StartUtc.Value,
            chainKey.OrganizationId,
            chainKey.ProjectId,
            chainKey.ProcessDefinitionId,
            fromEvent.Value,
            toEvent.EventName.Value);

        if (edgeKeyResult.IsFailed)
        {
            return Result.Fail<EdgeTransition>(edgeKeyResult.Errors);
        }

        return Result.Ok(new EdgeTransition(
            chainKey,
            edgeKeyResult.Value,
            toEvent.ActorUserId,
            toEvent.ActorAnonymousId,
            entryEventName));
    }
}
