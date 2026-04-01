using Enma.Admin.Application.Contracts;
using Enma.Common.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.Admin.Infrastructure.Caching.Services;

internal sealed class RedisEventDefinitionCacheInvalidator : IEventDefinitionCacheInvalidator
{
    private readonly IDatabase _database;
    private readonly string _keyPrefix;

    public RedisEventDefinitionCacheInvalidator(
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisOptions> options)
    {
        _database = connectionMultiplexer.GetDatabase(options.Value.Database);
        _keyPrefix = options.Value.KeyPrefix?.Trim() ?? string.Empty;
    }

    public async Task InvalidateAsync(Guid orgId, Guid projectId, CancellationToken ct = default)
    {
        var key = BuildKey(orgId, projectId);
        await _database.KeyDeleteAsync(key);
    }

    private RedisKey BuildKey(Guid orgId, Guid projectId)
    {
        return string.IsNullOrWhiteSpace(_keyPrefix)
            ? $"event-defs:{orgId}:{projectId}"
            : $"{_keyPrefix}:event-defs:{orgId}:{projectId}";
    }
}
