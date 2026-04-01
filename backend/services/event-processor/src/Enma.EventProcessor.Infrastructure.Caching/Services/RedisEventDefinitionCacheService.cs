using Enma.Common.Options;
using Enma.EventProcessor.Application.Contracts;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Enma.EventProcessor.Infrastructure.Caching.Services;

internal sealed class RedisEventDefinitionCacheService : IEventDefinitionCacheService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    private readonly IDatabase _database;
    private readonly string _keyPrefix;

    public RedisEventDefinitionCacheService(
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisOptions> options)
    {
        _database = connectionMultiplexer.GetDatabase(options.Value.Database);
        _keyPrefix = options.Value.KeyPrefix?.Trim() ?? string.Empty;
    }

    public async Task<HashSet<string>?> GetAllowedNamesAsync(Guid orgId, Guid projectId, CancellationToken ct = default)
    {
        var key = BuildKey(orgId, projectId);

        if (!await _database.KeyExistsAsync(key))
        {
            return null;
        }

        var members = await _database.SetMembersAsync(key);
        var set = new HashSet<string>(members.Length, StringComparer.Ordinal);

        foreach (var member in members)
        {
            if (member.HasValue)
            {
                set.Add(member.ToString());
            }
        }

        return set;
    }

    public async Task SetAllowedNamesAsync(Guid orgId, Guid projectId, IReadOnlyList<string> names, CancellationToken ct = default)
    {
        var key = BuildKey(orgId, projectId);
        var transaction = _database.CreateTransaction();

        _ = transaction.KeyDeleteAsync(key);

        if (names.Count > 0)
        {
            var values = names.Select(n => (RedisValue)n).ToArray();
            _ = transaction.SetAddAsync(key, values);
        }

        _ = transaction.KeyExpireAsync(key, CacheTtl);

        await transaction.ExecuteAsync();
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
