using Microsoft.Extensions.Logging;
using Moq;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Common.Interfaces.Messaging.Publishers;
using TradingService.Application.Common.Interfaces.Persistence;
using TradingService.Application.Features.Trades.Commands.ExecuteTrade;
using TradingService.Domain.Entities;
using TradingService.Domain.Events;
using TradingService.Domain.Repositories;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Application.Features.Trades.Commands.ExecuteTrade;

[Collection("TradeCollection")]
public class ExecuteTradeCommandHandlerTests : IClassFixture<TradeFixture>
{
    private readonly Mock<ITradeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ITradeExecutedEventPublisher> _publisherMock;
    private readonly Mock<ILogger<ExecuteTradeCommandHandler>> _loggerMock;
    private readonly ExecuteTradeCommandHandler _handler;
    private readonly Trade _trade;

    public ExecuteTradeCommandHandlerTests(TradeFixture tradeFixture)
    {
        _repositoryMock = new Mock<ITradeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cacheServiceMock = new Mock<ICacheService>();
        _publisherMock = new Mock<ITradeExecutedEventPublisher>();
        _loggerMock = new Mock<ILogger<ExecuteTradeCommandHandler>>();

        _handler = new ExecuteTradeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);

        _trade = tradeFixture.Trade;
    }

    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new ExecuteTradeCommandHandler(
            null!,
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("repository", act);
    }

    [Fact]
    public void Constructor_WhenUnitOfWorkIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new ExecuteTradeCommandHandler(
            _repositoryMock.Object,
            null!,
            _cacheServiceMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("unitOfWork", act);
    }

    [Fact]
    public void Constructor_WhenCacheServiceIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new ExecuteTradeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            null!,
            _publisherMock.Object,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("cacheService", act);
    }

    [Fact]
    public void Constructor_WhenPublisherIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new ExecuteTradeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            null!,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("publisher", act);
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new ExecuteTradeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            _publisherMock.Object,
            null!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", act);
    }

    [Fact]
    public async Task Handle_WhenCalled_AddsTradeToRepository()
    {
        // Arrange
        var command = new ExecuteTradeCommand(
            _trade.Side,
            _trade.Quantity,
            _trade.Price);

        Trade capturedTrade = null!;

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
            .Callback<Trade, CancellationToken>((t, ct) => capturedTrade = t );
        
        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedTrade);

        var capturedTradeId = Assert.IsType<Guid>(capturedTrade.Id);
        Assert.NotEqual(Guid.Empty, capturedTradeId);
        
        Assert.Equal(_trade.Side, capturedTrade.Side);
        Assert.Equal(_trade.Quantity, capturedTrade.Quantity);
        Assert.Equal(_trade.Price, capturedTrade.Price);
        Assert.Equal(_trade.TotalAmount, capturedTrade.TotalAmount);

        var capturedTradeExecutedAt = Assert.IsType<DateTime>(capturedTrade.ExecutedAt);
        Assert.InRange(capturedTradeExecutedAt, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow.AddSeconds(1));

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<Trade>(), It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(x => x.PublishAsync(It.IsAny<TradeExecutedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
