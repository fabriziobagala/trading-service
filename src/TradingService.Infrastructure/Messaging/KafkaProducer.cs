using System.Net.Mime;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingService.Application.Common.Interfaces.Messaging;
using TradingService.Infrastructure.Logging;

namespace TradingService.Infrastructure.Messaging;

public class KafkaProducer : IMessageProducer, IDisposable
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<string, string> _producer;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaProducer"/> class.
    /// </summary>
    /// <param name="options">The Kafka options.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> or <paramref name="logger"/> is <see langword="null"/>.</exception>
    public KafkaProducer(
        IOptions<KafkaOptions> options,
        ILogger<KafkaProducer> logger)
    {
        var optionsValue = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var config = new ProducerConfig
        {
            BootstrapServers = optionsValue.BootstrapServers,
            Acks = Acks.All // Wait for confirmation from all brokers.
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when the <paramref name="topic"/>, <paramref name="key"/>, or <paramref name="value"/> is empty or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="topic"/>, <paramref name="key"/>, or <paramref name="value"/> is <see langword="null"/>.</exception>
    public async Task ProduceAsync(
        string? topic,
        string? key,
        string? value,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _logger.LogKafkaProducerProducing(topic);

        try
        {
            var message = new Message<string, string>
            {
                Key = key,
                Value = value,
                Headers = new Headers
                {
                    { KafkaHeaderNames.ContentType, Encoding.UTF8.GetBytes(MediaTypeNames.Application.Json) },
                    { KafkaHeaderNames.ContentEncoding, Encoding.UTF8.GetBytes(Encoding.UTF8.WebName) },
                    { KafkaHeaderNames.Timestamp, BitConverter.GetBytes(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) }
                }
            };

            await _producer.ProduceAsync(topic, message, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogKafkaProducerProduced(topic, key, value);
        }
        catch (Exception ex)
        {
            _logger.LogKafkaProducerError(ex, topic, key, value, ex.Message);
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _producer.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
