using TradingService.Application.Features.Trades.Commands.ExecuteTrade;
using TradingService.Domain.Entities;

namespace TradingService.Application.Features.Trades.Mappers;

/// <summary>
/// Provides mapping functionality to convert trade commands to trade entities.
/// </summary>
public static class TradeCommandMapper
{
    /// <summary>
    /// Converts an <see cref="ExecuteTradeCommand"/> to a <see cref="Trade"/> entity.
    /// </summary>
    /// <param name="command">The trade command to convert.</param>
    /// <returns>A <see cref="Trade"/> entity with values mapped from the command and additional calculated fields.</returns>
    public static Trade ToEntity(this ExecuteTradeCommand command)
    {
        return new Trade
        {
            Id = Guid.NewGuid(),
            Side = command.Side,
            Quantity = command.Quantity,
            Price = command.Price,
            TotalAmount = command.Quantity * command.Price,
            ExecutedAt = DateTime.UtcNow
        };
    }
}
