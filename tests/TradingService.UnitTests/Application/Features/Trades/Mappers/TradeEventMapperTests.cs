using TradingService.Application.Features.Trades.Mappers;
using TradingService.Domain.Entities;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Application.Features.Trades.Mappers;

[Collection("TradeCollection")]
public class TradeEventMapperTests : IClassFixture<TradeFixture>
{
    private readonly Trade _trade;

    public TradeEventMapperTests(TradeFixture tradeFixture)
    {
        _trade = tradeFixture.Trade;
    }

    [Fact]
    public void ToExecutedEvent_MapsTradeToTradeExecutedEvent()
    {
        // Act
        var tradeExecutedEvent = _trade.ToExecutedEvent();

        // Assert
        Assert.Equal(_trade.Id, tradeExecutedEvent.Id);
        Assert.Equal(_trade.Side, tradeExecutedEvent.Side);
        Assert.Equal(_trade.Quantity, tradeExecutedEvent.Quantity);
        Assert.Equal(_trade.Price, tradeExecutedEvent.Price);
        Assert.Equal(_trade.TotalAmount, tradeExecutedEvent.TotalAmount);
        Assert.Equal(_trade.ExecutedAt, tradeExecutedEvent.ExecutedAt);
    }
}
