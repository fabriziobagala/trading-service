using TradingService.Application.Common.Interfaces.Persistence;
using TradingService.Infrastructure.Persistence.Context;

namespace TradingService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TradingDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> is <see langword="null"/>.</exception>
    public UnitOfWork(TradingDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    /// <exception cref="DbUpdateException">Thrown when an error occurs while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown when a concurrency violation occurs.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
