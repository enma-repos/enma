using Enma.BucketBuilder.Application.Models;
using Enma.BucketBuilder.Application.ValueObjects;
using FluentResults;

namespace Enma.BucketBuilder.Application.Abstractions;

public interface IWindowBucketBuildService
{
    Task<Result<WindowProjectionBatch>> BuildWindowAsync(BucketWindow? window, CancellationToken ct = default);
}
