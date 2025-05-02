namespace TradingService.Application.Common.Interfaces.Caching;

/// <summary>
/// Interface for a caching service.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Asynchronously retrieves a cached item by its key.
    /// </summary>
    /// <typeparam name="T">The type of the cached item to retrieve.</typeparam>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that contains the cached item of type <typeparamref name="T"/> if found; otherwise, <see langword="null"/>.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously sets a value in the cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
}
