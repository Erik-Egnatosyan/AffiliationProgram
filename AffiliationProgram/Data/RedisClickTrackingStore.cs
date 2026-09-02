using System.Text.Json;
using AffiliationProgram.Configs;
using AffiliationProgram.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AffiliationProgram.Data;

public sealed class RedisClickTrackingStore : IClickTrackingStore
{
    private const string KeyPref = "affiliate:track:";

    private readonly IDatabase _db;
    private readonly TimeSpan _ttl;

    public RedisClickTrackingStore(
        IConnectionMultiplexer redis,
        IOptions<AffiliateOptions> options)
    {
        _db = redis.GetDatabase();
        _ttl = options.Value.Expiration;
    }

    public async Task SaveAsync(Guid trackingId, TrackedClick click, CancellationToken ct)
    {
        var payload = JsonSerializer.Serialize(click);
        await _db.StringSetAsync(KeyPref + trackingId, payload, _ttl);
    }

    public async Task<TrackedClick?> GetAsync(Guid trackingId, CancellationToken ct)
    {
        var payload = await _db.StringGetAsync(KeyPref + trackingId);

        if (!payload.HasValue) return null;

        return JsonSerializer.Deserialize<TrackedClick>(payload.ToString());
    }

    public async Task RemoveAsync(Guid trackingId, CancellationToken ct)
    {
        await _db.KeyDeleteAsync(KeyPref + trackingId);
    }
}