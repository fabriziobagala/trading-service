using TradingService.Application.Dtos;
using TradingService.Domain.Entities;

namespace TradingService.Application.Features.Trades.Mappers;

/// <summary>
/// Provides mapping functionality to convert trade entities to trade DTOs and vice versa.
/// </summary>
public static class TradeDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Trade"/> entity to a <see cref="TradeDto"/>.
    /// </summary>
    /// <param name="trade">The trade entity to convert.</param>
    /// <returns>A <see cref="TradeDto"/> with values mapped from the entity.</returns>
    public static TradeDto ToDto(this Trade trade)
    {
        return new TradeDto
        {
            Id = trade.Id,
            Side = trade.Side,
            Quantity = trade.Quantity,
            Price = trade.Price,
            TotalAmount = trade.TotalAmount,
            ExecutedAt = trade.ExecutedAt
        };
    }

    /// <summary>
    /// Converts a <see cref="TradeDto"/> to a <see cref="Trade"/> entity.
    /// </summary>
    /// <param name="tradeDto">The trade DTO to convert.</param>
    /// <returns>A <see cref="Trade"/> entity with values mapped from the DTO.</returns>
    public static Trade ToEntity(this TradeDto tradeDto)
    {
        return new Trade
        {
            Id = tradeDto.Id,
            Side = tradeDto.Side,
            Quantity = tradeDto.Quantity,
            Price = tradeDto.Price,
            TotalAmount = tradeDto.TotalAmount,
            ExecutedAt = tradeDto.ExecutedAt
        };
    }
}
