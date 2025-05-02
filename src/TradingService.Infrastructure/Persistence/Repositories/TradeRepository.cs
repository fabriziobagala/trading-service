using Microsoft.EntityFrameworkCore;
using TradingService.Domain.Entities;
using TradingService.Domain.Repositories;
using TradingService.Infrastructure.Persistence.Context;

namespace TradingService.Infrastructure.Persistence.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly TradingDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TradeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> is <see langword="null"/>.</exception>
    public TradeRepository(TradingDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    public async Task AddAsync(
        Trade trade,
        CancellationToken cancellationToken = default)
    {
        await _context.Trades
            .AddAsync(trade, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">Thrown when more than one trade is found.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    public async Task<Trade?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Trades
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    public async Task<(IReadOnlyList<Trade> Items, int TotalCount)> GetPagedTradesAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Trades
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        var items = await _context.Trades
            .OrderByDescending(t => t.ExecutedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
            
        return (items, totalCount);
    }
}
