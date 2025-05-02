using Confluent.Kafka;

namespace TradingService.Consumer.Configuration;

/// <summary>
/// Configuration options for Kafka connectivity and consumer settings.
/// </summary>
public record class KafkaOptions
{
    /// <summary>
    /// Connection string for Kafka server.
    /// </summary>
    public string? BootstrapServers { get; set; }

    /// <summary>
    /// Client identifier for this application.
    /// </summary>
    public string? GroupId { get; set; }

    /// <summary>
    /// Auto offset reset policy for the consumer.
    /// </summary>
    public AutoOffsetReset AutoOffsetReset { get; set; }

    /// <summary>
    /// Topic for trade executed messages.
    /// </summary>
    public string? TradeExecutedTopic { get; set; }

    /// <summary>
    /// Allows auto-creation of topics if they do not exist.
    /// </summary>
    public bool AllowAutoCreateTopics { get; set; }
}
