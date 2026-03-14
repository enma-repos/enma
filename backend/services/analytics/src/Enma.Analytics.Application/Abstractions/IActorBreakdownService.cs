using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IActorBreakdownService
{
    Task<Result<ActorBreakdownDto>> GetActorBreakdownAsync(
        ProcessFilter filter, CancellationToken ct = default);
}
