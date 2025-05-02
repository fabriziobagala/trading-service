using Microsoft.EntityFrameworkCore;
using TradingService.Domain.Entities;

namespace TradingService.Infrastructure.Persistence.Context;

/// <summary>
/// A <see cref="DbContext"/> for the trading database.
/// </summary>
public class TradingDbContext : DbContext
{
    public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// A <see cref="DbSet"/> of trades stored in the database.
    /// </summary>
    public DbSet<Trade> Trades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
