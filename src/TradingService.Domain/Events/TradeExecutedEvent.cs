using TradingService.Domain.Enums;

namespace TradingService.Domain.Events;

/// <summary>
/// Represents an event that is triggered when a trade is executed.
/// </summary>
public record class TradeExecutedEvent
{
    /// <summary>
    /// The unique identifier for the trade.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The side of the trade.
    /// </summary>
    public TradeSide Side { get; set; }

    /// <summary>
    /// The quantity of the asset traded.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The price at which the asset was traded.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The total amount for the trade.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The date and time when the trade was executed.
    /// </summary>
    public DateTime ExecutedAt { get; set; }
}
