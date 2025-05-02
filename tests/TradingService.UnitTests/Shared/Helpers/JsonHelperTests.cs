using TradingService.Domain.Entities;
using TradingService.Shared.Helpers;
using TradingService.UnitTests.Fixtures;

namespace TradingService.UnitTests.Shared.Helpers;

[Collection("TradeCollection")]
public class JsonHelperTests : IClassFixture<TradeFixture>
{
    private const string TradeJsonString = "{"
        + "\"id\":\"6edad213-9259-4a4f-8a50-8a7615c58e03\","
        + "\"side\":\"Buy\","
        + "\"quantity\":10,"
        + "\"price\":123.45,"
        + "\"totalAmount\":1234.50,"
        + "\"executedAt\":\"2025-01-03T12:00:00Z\""
        + "}";

    private readonly Trade _trade;

    public JsonHelperTests(TradeFixture tradeFixture)
    {
        _trade = tradeFixture.Trade;
    }

    [Fact]
    public async Task SerializeAsync_WhenObjectIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Trade? trade = null;

        // Act
        var act = () => JsonHelper.SerializeAsync(trade);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>("obj", act);
    }

    [Fact]
    public async Task SerializeAsync_ValidObject_ReturnsJsonString()
    {
        // Act
        var actual = await JsonHelper.SerializeAsync(_trade);

        // Assert
        Assert.Equal(TradeJsonString, actual);
    }

    [Fact]
    public async Task DeserializeAsync_WhenJsonIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string? json = null;

        // Act
        var act = () => JsonHelper.DeserializeAsync<object>(json);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>("json", act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeserializeAsync_WhenJsonIsEmptyOrWhitespace_ThrowsArgumentException(string json)
    {
        // Act
        var act = () => JsonHelper.DeserializeAsync<object>(json);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>("json", act);
    }

    [Fact]
    public async Task DeserializeAsync_ValidJson_ReturnsDeserializedObject()
    {
        // Act
        var actual = await JsonHelper.DeserializeAsync<Trade>(TradeJsonString);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(_trade, actual);
    }
}
