namespace TradingService.Infrastructure.Caching.Redis;

/// <summary>
/// Configuration options for Redis caching.
/// </summary>
public record class RedisOptions
{
    /// <summary>
    /// Connection string for Redis server.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Prefix for cache keys.
    /// </summary>
    public string? InstanceName { get; set; }
}
