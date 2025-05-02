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

    [LoggerMessage(EventName = "KafkaProducerProducing", Level = LogLevel.Information, Message = "Producing message to Kafka topic: {Topic}")]
    public static partial void LogKafkaProducerProducing(this ILogger logger, string topic);

    [LoggerMessage(EventName = "KafkaProducerProduced", Level = LogLevel.Information, Message = "Produced message to Kafka topic: {Topic} with key: {Key} and value: {Value}")]
    public static partial void LogKafkaProducerProduced(this ILogger logger, string topic, string key, string value);

    [LoggerMessage(EventName = "KafkaProducerError", Level = LogLevel.Error, Message = "Error producing message to Kafka topic: {Topic} with key: {Key} and value: {Value}. Error: {Error}")]
    public static partial void LogKafkaProducerError(this ILogger logger, Exception ex, string topic, string key, string value, string error);

    [LoggerMessage(EventName = "TradeExecutedEventPublishing", Level = LogLevel.Information, Message = "Publishing TradeExecutedEvent with ID: {TradeExecutedEventId}")]
    public static partial void LogTradeExecutedEventPublishing(this ILogger logger, Guid tradeExecutedEventId);

    [LoggerMessage(EventName = "TradeExecutedEventPublished", Level = LogLevel.Information, Message = "Published TradeExecutedEvent with ID: {TradeExecutedEventId}")]
    public static partial void LogTradeExecutedEventPublished(this ILogger logger, Guid tradeExecutedEventId);
}
