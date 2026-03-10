using Enma.Common.Errors;
using FluentResults;

namespace Enma.BucketBuilder.Application.Models;

/// <summary>
/// Lease owner identity used to guard shard execution from concurrent workers.
/// </summary>
public sealed class LeaseToken
{
    public string Pipeline { get; }
    public ShardDescriptor Shard { get; }
    public string OwnerId { get; }
    public DateTime AcquiredAtUtc { get; }
    public DateTime ExpiresAtUtc { get; }

    private LeaseToken(
        string pipeline,
        ShardDescriptor shard,
        string ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
    {
        Pipeline = pipeline;
        Shard = shard;
        OwnerId = ownerId;
        AcquiredAtUtc = acquiredAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static Result<LeaseToken> Create(
        string? pipeline,
        ShardDescriptor? shard,
        string? ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
    {
        var errors = new List<IError>();

        var normalizedPipeline = ModelValidation.ValidateRequiredString(
            errors,
            pipeline,
            nameof(pipeline),
            minLength: 1,
            maxLength: 64);

        if (shard is null)
        {
            errors.Add(ApplicationErrors.Required(nameof(shard)));
        }

        var normalizedOwnerId = ModelValidation.ValidateRequiredString(
            errors,
            ownerId,
            nameof(ownerId),
            minLength: 1,
            maxLength: 200);

        ModelValidation.AddUtcDateTime(errors, acquiredAtUtc, nameof(acquiredAtUtc));
        ModelValidation.AddUtcDateTime(errors, expiresAtUtc, nameof(expiresAtUtc));

        if (expiresAtUtc <= acquiredAtUtc)
        {
            errors.Add(ApplicationErrors.Validation("expiresAtUtc must be greater than acquiredAtUtc."));
        }

        return errors.Count > 0
            ? Result.Fail<LeaseToken>(errors)
            : Result.Ok(new LeaseToken(normalizedPipeline, shard!, normalizedOwnerId, acquiredAtUtc, expiresAtUtc));
    }

    public static LeaseToken Rehydrate(
        string pipeline,
        ShardDescriptor shard,
        string ownerId,
        DateTime acquiredAtUtc,
        DateTime expiresAtUtc)
        => new LeaseToken(pipeline, shard, ownerId, acquiredAtUtc, expiresAtUtc);
}