using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathSourceEventReader
{
    Task<Result<IReadOnlyCollection<PathSourceEvent>>> GetWindowAsync(BucketWindow window, CancellationToken ct = default);
}
