using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IFlowGraphService
{
    Task<Result<FlowGraphDto>> GetFlowGraphAsync(ProcessFilter filter, CancellationToken ct = default);
}
