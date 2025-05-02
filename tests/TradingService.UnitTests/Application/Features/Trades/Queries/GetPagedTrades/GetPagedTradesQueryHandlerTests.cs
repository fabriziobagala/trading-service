using Microsoft.Extensions.Logging;
using Moq;
using TradingService.Application.Features.Trades.Queries.GetPagedTrades;
using TradingService.Domain.Entities;
using TradingService.Domain.Repositories;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Application.Features.Trades.Queries.GetPagedTrades;

[Collection("TradeCollection")]
public class GetPagedTradesQueryHandlerTests : IClassFixture<TradeFixture>
{
    private readonly Mock<ITradeRepository> _repositoryMock;
    private readonly Mock<ILogger<GetPagedTradesQueryHandler>> _loggerMock;
    private readonly GetPagedTradesQueryHandler _handler;
    private readonly Trade _trade;

    public GetPagedTradesQueryHandlerTests(TradeFixture tradeFixture)
    {
        _repositoryMock = new Mock<ITradeRepository>();
        _loggerMock = new Mock<ILogger<GetPagedTradesQueryHandler>>();

        _handler = new GetPagedTradesQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object);

        _trade = tradeFixture.Trade;
    }

    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new GetPagedTradesQueryHandler(
            null!,
            _loggerMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>("repository", act);
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new GetPagedTradesQueryHandler(
            _repositoryMock.Object,
            null!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", act);
    }

    [Fact]
    public async Task Handle_WhenNoTradesExist_ReturnsEmptyPaginatedResultDto()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var trades = new List<Trade>();
        var totalCount = trades.Count;

        _repositoryMock
            .Setup(repo => repo.GetPagedTradesAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync((trades, totalCount));

        var query = new GetPagedTradesQuery(pageNumber, pageSize);

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Empty(actual.Items);
        Assert.Equal(totalCount, actual.TotalCount);
        Assert.Equal(pageNumber, actual.PageNumber);
        Assert.Equal(pageSize, actual.PageSize);

        _repositoryMock.Verify(repo => repo.GetPagedTradesAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTradesExist_ReturnsPaginatedResultDto()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var trades = new List<Trade> { _trade };
        var totalCount = trades.Count;

        _repositoryMock
            .Setup(repo => repo.GetPagedTradesAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync((trades, totalCount));

        var query = new GetPagedTradesQuery(pageNumber, pageSize);

        // Act
        var actual = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(totalCount, actual.TotalCount);
        Assert.Equal(pageNumber, actual.PageNumber);
        Assert.Equal(pageSize, actual.PageSize);
        Assert.Single(actual.Items);

        _repositoryMock.Verify(repo => repo.GetPagedTradesAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()), Times.Once);
    }
}
