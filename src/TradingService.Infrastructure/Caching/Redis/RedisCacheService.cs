using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Infrastructure.Logging;
using TradingService.Shared.Helpers;

namespace TradingService.Infrastructure.Caching.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly DistributedCacheEntryOptions _defaultOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
    /// </summary>
    /// <param name="distributedCache">The distributed cache instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="distributedCache"/> or <paramref name="logger"/> is <see langword="null"/>.</exception>
    public RedisCacheService(
        IDistributedCache distributedCache,
        ILogger<RedisCacheService> logger)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _defaultOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty or whitespace.</exception>
    /// <exception cref="JsonException">Thrown when deserialization fails.</exception>
    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        _logger.LogRedisCacheRetrieving(key);

        var cachedData = await _distributedCache.GetStringAsync(key, cancellationToken)
            .ConfigureAwait(false);
            
        if (cachedData == null)
        {
            _logger.LogRedisCacheNotFound(key);
            return default;
        }
        
        var value = await JsonHelper.DeserializeAsync<T>(cachedData, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogRedisCacheRetrieved(key);

        return value;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="NotSupportedException">Thrown when the type of <paramref name="value"/> is not supported for serialization.</exception>
    public async Task SetAsync<T>(
        string key,
        T value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        
        _logger.LogRedisCacheSetting(key);

        var serializedValue = await JsonHelper.SerializeAsync(value, cancellationToken)
            .ConfigureAwait(false);

        await _distributedCache.SetStringAsync(key, serializedValue, _defaultOptions, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogRedisCacheSet(key, serializedValue);
    }
}
