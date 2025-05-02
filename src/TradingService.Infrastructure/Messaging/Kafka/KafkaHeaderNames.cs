namespace TradingService.Infrastructure.Messaging.Kafka;

/// <summary>
/// Provides a set of constants representing the names of Kafka headers.
/// </summary>
public static class KafkaHeaderNames
{
    /// <summary>
    /// The name of the content type header.
    /// </summary>
    public const string ContentType = "content-type";

    /// <summary>
    /// The name of the content encoding header.
    /// </summary>
    public const string ContentEncoding = "content-encoding";

    /// <summary>
    /// The name of the timestamp header.
    /// </summary>
    public const string Timestamp = "timestamp";
}
