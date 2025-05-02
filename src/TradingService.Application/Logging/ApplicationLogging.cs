using Microsoft.Extensions.Logging;

namespace TradingService.Application.Logging;

public static partial class ApplicationLogging
{
    [LoggerMessage(EventName = "RequestHandling", Level = LogLevel.Information, Message = "Handling {RequestName} with payload: {@Request}")]
    public static partial void LogRequestHandling(this ILogger logger, string requestName, object request);

    [LoggerMessage(EventName = "RequestHandled", Level = LogLevel.Information, Message = "Handled {RequestName}")]
    public static partial void LogRequestHandled(this ILogger logger, string requestName);

    [LoggerMessage(EventName = "NoTradesFound", Level = LogLevel.Warning, Message = "No trades found for the given page number: {PageNumber} and size: {PageSize}.")]
    public static partial void LogNoTradesFound(this ILogger logger, int pageNumber, int pageSize);

    [LoggerMessage(EventName = "RetrievedTrades", Level = LogLevel.Information, Message = "Retrieved {Count} trades")]
    public static partial void LogRetrievedTrades(this ILogger logger, int count);

    [LoggerMessage(EventName = "TradeNotFound", Level = LogLevel.Warning, Message = "Trade with ID {TradeId} not found")]
    public static partial void LogTradeNotFound(this ILogger logger, Guid tradeId);
}
