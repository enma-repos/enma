using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IFlowGraphService
{
    Task<Result<FlowGraphDto>> GetFlowGraphAsync(ProcessFilter filter, CancellationToken ct = default);

    Task<Result<FlowGraphDto>> GetFlowGraphByEntryEventAsync(
        ProcessFilter filter, string entryEventName, CancellationToken ct = default);

    Task<Result<FlowGraphDto>> GetMultiProcessFlowGraphAsync(
        MultiProcessFilter filter, CancellationToken ct = default);
}
