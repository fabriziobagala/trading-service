namespace TradingService.Infrastructure.Messaging.Kafka;

/// <summary>
/// Configuration options for Kafka messaging.
/// </summary>
public record class KafkaOptions
{
    /// <summary>
    /// Connection string for Kafka server.
    /// </summary>
    public string? BootstrapServers { get; set; }

    /// <summary>
    /// Topic name for trade executed messages.
    /// </summary>
    public string? TradeExecutedTopic { get; set; }
}
