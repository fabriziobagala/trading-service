using Microsoft.Extensions.Logging;

namespace TradingService.Consumer.Logging;

public static partial class ConsumerLogging
{
    [LoggerMessage(EventName = "TradeConsumerServiceStarting", Level = LogLevel.Information, Message = "Trade consumer service is starting...")]
    public static partial void LogTradeConsumerServiceStarting(this ILogger logger);

    [LoggerMessage(EventName = "TradeExecutedEventReceived", Level = LogLevel.Information, Message = "Received trade executed event with key {Key} from partition {Partition} at offset {Offset}")]
    public static partial void LogTradeExecutedEventReceived(this ILogger logger, string key, int partition, long offset);

    [LoggerMessage(EventName = "TradeExecutedEventDeserialized", Level = LogLevel.Information, Message = "Trade executed event with ID: {TradeId} Side: {Side} Quantity: {Quantity} Price: {Price} TotalAmount: {TotalAmount} ExecutedAt: {ExecutedAt}")]
    public static partial void LogTradeExecutedEventDeserialized(this ILogger logger, Guid tradeId, string side, int quantity, decimal price, decimal totalAmount, DateTime executedAt);
    
    [LoggerMessage(EventName = "TradeExecutedEventProcessingError", Level = LogLevel.Error, Message = "Error processing trade event: {ErrorMessage}")]
    public static partial void LogTradeExecutedEventProcessingError(this ILogger logger, Exception ex, string errorMessage);

    [LoggerMessage(EventName = "TradeConsumerServiceError", Level = LogLevel.Error, Message = "Trade consumer service encountered an error: {ErrorMessage}")]
    public static partial void LogTradeConsumerServiceError(this ILogger logger, Exception ex, string errorMessage);

    [LoggerMessage(EventName = "TradeConsumerServiceStopping", Level = LogLevel.Information, Message = "Trade consumer service is stopping.")]
    public static partial void LogTradeConsumerServiceStopping(this ILogger logger);
}
