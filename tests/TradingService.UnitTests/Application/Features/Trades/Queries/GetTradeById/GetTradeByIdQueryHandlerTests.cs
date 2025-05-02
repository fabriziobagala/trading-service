using Microsoft.Extensions.Logging;
using Moq;
using TradingService.Application.Common.Exceptions;
using TradingService.Application.Common.Interfaces.Caching;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Application.Features.Trades.Queries.GetTradeById;
using TradingService.Domain.Entities;
using TradingService.Domain.Repositories;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Application.Features.Trades.Queries.GetTradeById;

[Collection("TradeCollection")]
public class GetTradeByIdQueryHandlerTests : IClassFixture<TradeFixture>
{
    private readonly Mock<ITradeRepository> _repositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<GetTradeByIdQueryHandler>> _loggerMock;
    private readonly GetTradeByIdQueryHandler _handler;
    private readonly Trade _trade;

    public GetTradeByIdQueryHandlerTests(TradeFixture tradeFixture)
    {
        _repositoryMock = new Mock<ITradeRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<GetTradeByIdQueryHandler>>();
        _handler = new GetTradeByIdQueryHandler(_repositoryMock.Object, _cacheServiceMock.Object, _loggerMock.Object);
        _trade = tradeFixture.Trade;
    }

    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new GetTradeByIdQueryHandler(
            null!,
            _cacheServiceMock.Object,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", act);
    }

    [Fact]
    public void Constructor_WhenCacheServiceIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new GetTradeByIdQueryHandler(
            _repositoryMock.Object,
            null!,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("cacheService", act);
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new GetTradeByIdQueryHandler(
            _repositoryMock.Object,
            _cacheServiceMock.Object,
            null!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", act);
    }

    [Fact]
    public async Task Handle_WhenTradeExistsInCache_ReturnsTradeDto()
    {
        // Arrange
        var tradeId = _trade.Id;
        var query = new GetTradeByIdQuery(tradeId);
        var expected = _trade.ToDto();
        var cacheKey = $"trade:{tradeId}";

        _cacheServiceMock
            .Setup(cache => cache.GetAsync<TradeDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);

        _cacheServiceMock.Verify(cache => cache.GetAsync<TradeDto>(cacheKey, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(tradeId, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTradeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var tradeId = Guid.NewGuid();
        var query = new GetTradeByIdQuery(tradeId);

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Trade?)null);
        
        // Act
        var act = () => _handler.Handle(query, CancellationToken.None);
        
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);

        _cacheServiceMock.Verify(cache => cache.GetAsync<TradeDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(tradeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTradeExists_ReturnsTradeDto()
    {
        // Arrange
        var tradeId = _trade.Id;
        var query = new GetTradeByIdQuery(tradeId);
        var expected = _trade.ToDto();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(tradeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_trade);
        
        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(expected, actual);

        _cacheServiceMock.Verify(cache => cache.GetAsync<TradeDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(tradeId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
