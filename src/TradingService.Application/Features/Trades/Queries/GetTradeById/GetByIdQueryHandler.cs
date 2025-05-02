using MediatR;
using Microsoft.Extensions.Logging;
using TradingService.Application.Common.Exceptions;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Application.Logging;
using TradingService.Domain.Repositories;

namespace TradingService.Application.Features.Trades.Queries.GetTradeById;

public class GetByIdQueryHandler : IRequestHandler<GetTradeByIdQuery, TradeDto>
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetByIdQueryHandler> _logger;

    public GetByIdQueryHandler(
        ITradeRepository tradeRepository, 
        ICacheService cacheService,
        ILogger<GetByIdQueryHandler> logger)
    {
        _tradeRepository = tradeRepository ?? throw new ArgumentNullException(nameof(tradeRepository));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TradeDto> Handle(
        GetTradeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"trade:{request.Id}";
        
        var cachedTrade = await _cacheService.GetAsync<TradeDto>(cacheKey, cancellationToken)
            .ConfigureAwait(false);
        
        if (cachedTrade != null)
        {
            return cachedTrade;
        }

        var trade = await _tradeRepository.GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);
        
        if (trade == null)
        {
            _logger.LogTradeNotFound(request.Id);
            throw new NotFoundException($"Trade with ID {request.Id} not found");
        }

        var tradeDto = trade.ToDto();
        
        await _cacheService.SetAsync(cacheKey, tradeDto, cancellationToken)
            .ConfigureAwait(false);

        return tradeDto;
    }
}
