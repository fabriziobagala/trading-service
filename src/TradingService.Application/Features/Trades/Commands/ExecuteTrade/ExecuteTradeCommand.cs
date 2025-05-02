using MediatR;
using TradingService.Application.Dtos;
using TradingService.Domain.Enums;

namespace TradingService.Application.Features.Trades.Commands.ExecuteTrade;

public class ExecuteTradeCommand : IRequest<TradeDto>
{
    public TradeSide Side { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public ExecuteTradeCommand(TradeSide side, int quantity, decimal price)
    {
        Side = side;
        Quantity = quantity;
        Price = price;
    }
}
