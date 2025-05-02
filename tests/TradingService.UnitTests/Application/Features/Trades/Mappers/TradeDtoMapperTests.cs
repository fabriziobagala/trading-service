using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Domain.Entities;
using TradingService.Domain.Enums;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Application.Features.Trades.Mappers;

[Collection("TradeCollection")]
public class TradeDtoMapperTests : IClassFixture<TradeFixture>
{
    private readonly Trade _trade;

    public TradeDtoMapperTests(TradeFixture tradeFixture)
    {
        _trade = tradeFixture.Trade;
    }

    [Fact]
    public void ToDto_MapsTradeToTradeDto()
    {
        // Act
        var tradeDto = _trade.ToDto();

        // Assert
        Assert.Equal(_trade.Id, tradeDto.Id);
        Assert.Equal(_trade.Side, tradeDto.Side);
        Assert.Equal(_trade.Quantity, tradeDto.Quantity);
        Assert.Equal(_trade.Price, tradeDto.Price);
        Assert.Equal(_trade.TotalAmount, tradeDto.TotalAmount);
        Assert.Equal(_trade.ExecutedAt, tradeDto.ExecutedAt);
    }

    [Fact]
    public void ToEntity_MapsTradeDtoToTrade()
    {
        // Arrange
        var tradeDto = new TradeDto
        {
            Id = Guid.NewGuid(),
            Side = TradeSide.Sell,
            Quantity = 200,
            Price = 75.0m,
            TotalAmount = 15000.0m,
            ExecutedAt = DateTime.UtcNow
        };

        // Act
        var trade = tradeDto.ToEntity();

        // Assert
        Assert.Equal(tradeDto.Id, trade.Id);
        Assert.Equal(tradeDto.Side, trade.Side);
        Assert.Equal(tradeDto.Quantity, trade.Quantity);
        Assert.Equal(tradeDto.Price, trade.Price);
        Assert.Equal(tradeDto.TotalAmount, trade.TotalAmount);
        Assert.Equal(tradeDto.ExecutedAt, trade.ExecutedAt);
    }
}
