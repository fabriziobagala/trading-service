using TradingService.Application.Features.Trades.Commands.ExecuteTrade;
using TradingService.Application.Features.Trades.Mappers;
using TradingService.Domain.Enums;

namespace TradingService.UnitTests.Application.Features.Trades.Mappers;

public class TradeCommandMapperTests
{
    [Fact]
    public void ToEntity_MapsExecuteTradeCommandToTrade()
    {
        // Arrange
        var command = new ExecuteTradeCommand(TradeSide.Buy, 10, 100.0m);

        // Act
        var trade = command.ToEntity();

        // Assert
        Assert.NotNull(trade);

        var tradeId = Assert.IsType<Guid>(trade.Id);
        Assert.NotEqual(Guid.Empty, tradeId);
        
        Assert.Equal(command.Side, trade.Side);
        Assert.Equal(command.Quantity, trade.Quantity);
        Assert.Equal(command.Price, trade.Price);
        Assert.Equal(command.Quantity * command.Price, trade.TotalAmount);

        var tradeExecutedAt = Assert.IsType<DateTime>(trade.ExecutedAt);
        Assert.Equal(DateTimeKind.Utc, tradeExecutedAt.Kind);
        Assert.InRange(tradeExecutedAt, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow.AddSeconds(1));
    }
}
