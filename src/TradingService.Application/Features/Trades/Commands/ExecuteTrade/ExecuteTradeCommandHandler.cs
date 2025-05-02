using MediatR;
using Microsoft.Extensions.Logging;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Common.Interfaces.Messaging.Publishers;
using TradingService.Application.Common.Interfaces.Persistence;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Application.Logging;
using TradingService.Domain.Repositories;

namespace TradingService.Application.Features.Trades.Commands.ExecuteTrade;

public class ExecuteTradeCommandHandler : IRequestHandler<ExecuteTradeCommand, TradeDto>
{
    private readonly ITradeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ITradeExecutedEventPublisher _publisher;
    private readonly ILogger<ExecuteTradeCommandHandler> _logger;

    public ExecuteTradeCommandHandler(
        ITradeRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        ITradeExecutedEventPublisher publisher,
        ILogger<ExecuteTradeCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TradeDto> Handle(
        ExecuteTradeCommand request,
        CancellationToken cancellationToken)
    {
        var trade = request.ToEntity();

        await _repository.AddAsync(trade, cancellationToken)
            .ConfigureAwait(false);

        await _unitOfWork.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogTradeSaved(trade.Id);
        
        await _cacheService.SetAsync($"trade:{trade.Id}", trade, cancellationToken)
            .ConfigureAwait(false);
        
        var tradeExecutedEvent = trade.ToExecutedEvent();

        await _publisher.PublishAsync(tradeExecutedEvent, cancellationToken)
            .ConfigureAwait(false);
        
        var tradeDto = trade.ToDto();
        
        return tradeDto;
    }
}
