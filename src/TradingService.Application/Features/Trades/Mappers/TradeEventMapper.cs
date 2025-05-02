using TradingService.Domain.Entities;
using TradingService.Domain.Events;

namespace TradingService.Application.Features.Trades.Mappers;

/// <summary>
/// Provides mapping functionality to convert trade entities to trade events.
/// </summary>
public static class TradeEventMapper
{
    /// <summary>
    /// Converts a <see cref="Trade"/> to a <see cref="TradeExecutedEvent"/> event.
    /// </summary>
    /// <param name="trade">The trade event to map.</param>
    /// <returns>A <see cref="TradeExecutedEvent"/> with values mapped from the entity.</returns>
    public static TradeExecutedEvent ToExecutedEvent(this Trade trade)
    {
        return new TradeExecutedEvent
        {
            Id = trade.Id,
            Side = trade.Side,
            Quantity = trade.Quantity,
            Price = trade.Price,
            TotalAmount = trade.TotalAmount,
            ExecutedAt = trade.ExecutedAt
        };
    }
}
