namespace TradingService.Application.Common.Interfaces.Messaging;

/// <summary>
/// Interface for a message producer.
/// </summary>
public interface IMessageProducer
{
    /// <summary>
    /// Asynchronously produces a message to the specified Kafka topic.
    /// </summary>
    /// <param name="topic">The Kafka topic to produce the message to.</param>
    /// <param name="key">The message key to produce.</param>
    /// <param name="value">The message value to produce.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProduceAsync(
        string? topic,
        string? key,
        string? value,
        CancellationToken cancellationToken = default);
}
