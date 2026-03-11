using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;

namespace Enma.BucketBuilder.Application.Contracts.Persistence;

public interface IPathSourceEventReader
{
    Task<IReadOnlyCollection<PathSourceEvent>> GetWindowAsync(BucketWindow window, CancellationToken ct = default);
}
