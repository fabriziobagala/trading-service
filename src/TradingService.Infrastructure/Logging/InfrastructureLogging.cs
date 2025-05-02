using Microsoft.Extensions.Logging;

namespace TradingService.Infrastructure.Logging;

public static partial class InfrastructureLogging
{
    [LoggerMessage(EventName = "RedisCacheRetrieving", Level = LogLevel.Information, Message = "Retrieving data from Redis cache for key: {Key}")]
    public static partial void LogRedisCacheRetrieving(this ILogger logger, string key);

    [LoggerMessage(EventName = "RedisCacheNotFound", Level = LogLevel.Warning, Message = "Redis cache miss for key: {Key}")]
    public static partial void LogRedisCacheNotFound(this ILogger logger, string key);

    [LoggerMessage(EventName = "RedisCacheRetrieved", Level = LogLevel.Information, Message = "Data retrieved from Redis cache for key: {Key}")]
    public static partial void LogRedisCacheRetrieved(this ILogger logger, string key);

    [LoggerMessage(EventName = "RedisCacheSetting", Level = LogLevel.Information, Message = "Setting data in Redis cache for key: {Key}")]
    public static partial void LogRedisCacheSetting(this ILogger logger, string key);

    [LoggerMessage(EventName = "RedisCacheSet", Level = LogLevel.Information, Message = "Data set in Redis cache for key: {Key} with value: {Value}")]
    public static partial void LogRedisCacheSet(this ILogger logger, string key, string value);
}
