using TradingService.Domain.Events;

namespace TradingService.Application.Common.Interfaces.Messaging.Publishers;

/// <summary>
/// Interface for publishing trade executed events.
/// </summary>
public interface ITradeExecutedEventPublisher
{
    /// <summary>
    /// Asynchronously publishes a <see cref="TradeExecutedEvent"/> to the messaging system.
    /// </summary>
    /// <param name="tradeExecutedEvent">The trade executed event to publish.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PublishAsync(
        TradeExecutedEvent tradeExecutedEvent,
        CancellationToken cancellationToken = default);
}
