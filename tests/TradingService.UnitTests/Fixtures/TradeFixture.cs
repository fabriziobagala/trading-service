using TradingService.Domain.Entities;
using TradingService.Domain.Enums;

namespace TradingService.UnitTests.Fixtures;

public class TradeFixture
{
    public Trade Trade { get; set; }

    public TradeFixture()
    {
        Trade = new Trade
        {
            Id = Guid.Parse("6edad213-9259-4a4f-8a50-8a7615c58e03"),
            Side = TradeSide.Buy,
            Quantity = 10,
            Price = 123.45m,
            TotalAmount = 10 * 123.45m,
            ExecutedAt = new DateTime(2025, 1, 3, 12, 0, 0, DateTimeKind.Utc),
        };
    }
}
