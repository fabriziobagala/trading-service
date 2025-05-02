using TradingService.Domain.Entities;

namespace TradingService.Domain.Repositories;

/// <summary>
/// Interface for the trade repository.
/// </summary>
public interface ITradeRepository
{
    /// <summary>
    /// Asynchronously adds a new trade.
    /// </summary>
    /// <param name="trade">The trade entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAsync(
        Trade trade,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Asynchronously retrieves a trade by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the trade to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that contains the trade entity if found; otherwise, <see langword="null"/>.</returns>
    Task<Trade?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a paginated list of trades.
    /// </summary>
    /// <param name="pageNumber">The number of the page to retrieve (1-based index).</param>
    /// <param name="pageSize">The size of the page (number of items per page).</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that contains a tuple with:
    /// <list type="bullet">
    ///    <item>Items: A read-only list of trades for the requested page.</item>
    ///    <item>TotalCount: The total number of trades available.</item>
    /// </list>
    /// </returns>
    Task<(IReadOnlyList<Trade> Items, int TotalCount)> GetPagedTradesAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}
