using MediatR;
using Microsoft.Extensions.Logging;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Application.Logging;
using TradingService.Domain.Repositories;
using TradingService.Shared.Models;

namespace TradingService.Application.Features.Trades.Queries.GetPagedTrades;

public class GetPagedTradesQueryHandler : IRequestHandler<GetPagedTradesQuery, PaginatedResult<TradeDto>>
{
    private readonly ITradeRepository _repository;
    private readonly ILogger<GetPagedTradesQueryHandler> _logger;

    public GetPagedTradesQueryHandler(
        ITradeRepository repository,
        ILogger<GetPagedTradesQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaginatedResult<TradeDto>> Handle(
        GetPagedTradesQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetPagedTradesAsync(request.PageNumber, request.PageSize, cancellationToken)
            .ConfigureAwait(false);
        
        if (!items.Any())
        {
            _logger.LogNoTradesFound(request.PageNumber, request.PageSize);
            return new PaginatedResult<TradeDto>([], 0, request.PageNumber, request.PageSize);
        }
        
        var tradeDtos = items.Select(trade => trade.ToDto());

        _logger.LogRetrievedTrades(tradeDtos.Count());
        return new PaginatedResult<TradeDto>(tradeDtos, totalCount, request.PageNumber, request.PageSize);
    }
}
