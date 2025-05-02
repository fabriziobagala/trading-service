using Microsoft.Extensions.Logging;

namespace TradingService.Application.Logging;

public static partial class ApplicationLogging
{

    [LoggerMessage(EventName = "NoTradesFound", Level = LogLevel.Warning, Message = "No trades found for the given page number: {PageNumber} and size: {PageSize}.")]
    public static partial void LogNoTradesFound(this ILogger logger, int pageNumber, int pageSize);

    [LoggerMessage(EventName = "RetrievedTrades", Level = LogLevel.Information, Message = "Retrieved {Count} trades")]
    public static partial void LogRetrievedTrades(this ILogger logger, int count);
}
