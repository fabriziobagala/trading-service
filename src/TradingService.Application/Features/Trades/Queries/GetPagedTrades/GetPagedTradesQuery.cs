using MediatR;
using TradingService.Application.Dtos;
using TradingService.Shared.Models;

namespace TradingService.Application.Features.Trades.Queries.GetPagedTrades;

public class GetPagedTradesQuery : IRequest<PaginatedResult<TradeDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetPagedTradesQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
