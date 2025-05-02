using MediatR;
using TradingService.Application.Dtos;

namespace TradingService.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQuery : IRequest<TradeDto>
{
    public Guid Id { get; set; }

    public GetTradeByIdQuery(Guid id)
    {
        Id = id;
    }
}
