using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingService.Application.Common.Interfaces.Messaging;
using TradingService.Application.Common.Interfaces.Messaging.Publishers;
using TradingService.Domain.Events;
using TradingService.Infrastructure.Logging;
using TradingService.Shared.Helpers;

namespace TradingService.Infrastructure.Messaging.Publishers;

public class KafkaTradeExecutedEventPublisher : ITradeExecutedEventPublisher
{
    private readonly IMessageProducer _producer;
    private readonly KafkaOptions _options;
    private readonly ILogger<KafkaTradeExecutedEventPublisher> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaTradeExecutedEventPublisher"/> class.
    /// </summary>
    /// <param name="kafkaProducer">The Kafka producer instance.</param>
    /// <param name="options">The Kafka options.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="kafkaProducer"/>, <paramref name="options"/>, or <paramref name="logger"/> is <see langword="null"/>.</exception>
    public KafkaTradeExecutedEventPublisher(
        IMessageProducer kafkaProducer,
        IOptions<KafkaOptions> options,
        ILogger<KafkaTradeExecutedEventPublisher> logger)
    {
        _producer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tradeExecutedEvent"/> is <see cref="null"/>.</exception>
    public async Task PublishAsync(
        TradeExecutedEvent tradeExecutedEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tradeExecutedEvent);

        _logger.LogTradeExecutedEventPublishing(tradeExecutedEvent.Id);

        var message = await JsonHelper.SerializeAsync(tradeExecutedEvent, cancellationToken)
            .ConfigureAwait(false);

        await _producer.ProduceAsync(_options.TradeExecutedTopic, tradeExecutedEvent.Id.ToString(), message, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogTradeExecutedEventPublished(tradeExecutedEvent.Id);
    }
}
