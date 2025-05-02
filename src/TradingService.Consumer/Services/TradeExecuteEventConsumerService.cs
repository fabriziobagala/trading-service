using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingService.Consumer.Configuration;
using TradingService.Consumer.Logging;
using TradingService.Domain.Events;
using TradingService.Shared.Helpers;

namespace TradingService.Consumer.Services;

public class TradeExecuteEventConsumerService : BackgroundService
{
    private readonly KafkaOptions _options;
    private readonly ILogger<TradeExecuteEventConsumerService> _logger;
    private readonly IConsumer<string, string> _consumer;
    
    public TradeExecuteEventConsumerService(
        IOptions<KafkaOptions> settings,
        ILogger<TradeExecuteEventConsumerService> logger)
    {
        _options = settings.Value;
        _logger = logger;
        
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.GroupId,
            AutoOffsetReset = _options.AutoOffsetReset,
            AllowAutoCreateTopics = _options.AllowAutoCreateTopics
        };
        
        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogTradeConsumerServiceStarting();

        _consumer.Subscribe(_options.TradeExecutedTopic);
        
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    _logger.LogTradeExecutedEventReceived(consumeResult.Message.Key, consumeResult.Partition, consumeResult.Offset);
                    
                    var trade = await JsonHelper.DeserializeAsync<TradeExecutedEvent>(consumeResult.Message.Value, stoppingToken)
                        .ConfigureAwait(false);
                    
                    if (trade != null)
                    {
                        _logger.LogTradeExecutedEventDeserialized(
                            trade.Id,
                            trade.Side.ToString(),
                            trade.Quantity,
                            trade.Price,
                            trade.TotalAmount,
                            trade.ExecutedAt);
                    }
                    
                    _consumer.Commit(consumeResult);

                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken)
                        .ConfigureAwait(false);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogTradeExecutedEventProcessingError(ex, ex.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when stopping service
        }
        catch (Exception ex)
        {
            _logger.LogTradeConsumerServiceError(ex, ex.Message);
        }
        finally
        {
            _consumer.Close();
            _consumer.Dispose();

            _logger.LogTradeConsumerServiceStopping();
        }
    }
}
