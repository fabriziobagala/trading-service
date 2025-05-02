using MediatR;
using Microsoft.Extensions.Logging;
using TradingService.Application.Common.Exceptions;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Application.Logging;
using TradingService.Domain.Repositories;

namespace TradingService.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryHandler : IRequestHandler<GetTradeByIdQuery, TradeDto>
{
    private readonly ITradeRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetTradeByIdQueryHandler> _logger;

    public GetTradeByIdQueryHandler(
        ITradeRepository repository,
        ICacheService cacheService,
        ILogger<GetTradeByIdQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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

        var trade = await _repository.GetByIdAsync(request.Id, cancellationToken)
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
