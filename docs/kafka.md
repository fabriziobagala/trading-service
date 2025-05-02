# Kafka and Messaging Documentation

This document describes the Kafka integration, covering producer configuration, message formats, and consumer implementation details.

## Architecture Overview

The Trading Service implements an event-driven architecture using Apache Kafka:

- **Producer**: The API service publishes trade execution events to Kafka when trades are created
- **Consumer**: A separate console application consumes these events asynchronously
- **Message Format**: JSON-serialized trade execution details with standardized headers

## Producer Implementation

Trade execution events are published to Kafka by an internal producer within the Infrastructure layer.

Topic: `trade-executed`

The topic name is configured via the `Kafka__TradeExecutedTopic` environment variable.

### Message Key

The message key is a UUID string representing the unique identifier of the trade:

```text
2e1ac66f-49f4-4ea1-94ba-a817c5b725e2
```

### Message Value

A JSON object with the full trade execution details:

```json
{
  "id": "2e1ac66f-49f4-4ea1-94ba-a817c5b725e2",
  "side": "Buy",
  "quantity": 100,
  "price": 75.50,
  "totalAmount": 7550.00,
  "executedAt": "2025-05-02T16:24:13.2937876Z"
}
```

### Message Headers

```json
{
  "content-encoding": "utf-8",
  "content-type": "application/json",
  "timestamp": "1746211680727"
}
```

- `content-encoding`: Encoding format of the message body (UTF-8)
- `content-type`: MIME type of the message (application/json)
- `timestamp`: Unix timestamp in milliseconds (UTC)

### Serialization

- **Format**: UTF-8 encoded JSON
- **Schema**: Follows the same contract as the `GET /trades/{id}` API response
- **Type Handling**: Uses System.Text.Json with enums serialized as strings

### Producer Code Structure

The producer is implemented in the Infrastructure layer:

- [KafkaProducer](src/TradingService.Infrastructure/Messaging/KafkaProducer.cs): Generic implementation of IMessageProducer
- [KafkaTradeExecutedEventPublisher](src/TradingService.Infrastructure/Messaging/Publishers/KafkaTradeExecutedEventPublisher.cs): Specific publisher for trade events
- [TradeEventMapper](src/TradingService.Application/Features/Trades/Mappers/TradeEventMapper.cs): Maps domain entities to event DTOs

### Configuration

Producer options are configured via environment variables:

```text
KAFKA_BOOTSTRAP_SERVERS
KAFKA_TRADE_EXECUTED_TOPIC
```

## Consumer Implementation

The solution includes a separate console application (`TradingService.Consumer`) that processes trade execution events:

- Runs as a hosted background service using `BackgroundService`
- Subscribes to the `trade-executed` topic
- Deserializes and logs the messages to the console

### Consumer Service

The main consumer service is implemented in [TradeConsumerService.cs](src/TradingService.Consumer/Services/TradeConsumerService.cs) with the following features:

- Inherits from `BackgroundService` for long-running background processing
- Uses the `Confluent.Kafka` client library with consumer groups
- Implements graceful shutdown on application termination
- Provides error handling and retry capabilities
- Uses source-generated logging for high performance

### Configuration Options

The consumer is configured via environment variables:

```text
KAFKA_BOOTSTRAP_SERVERS
KAFKA_GROUP_ID
KAFKA_AUTO_OFFSET_RESET
KAFKA_ALLOW_AUTO_CREATE_TOPICS
KAFKA_TRADE_EXECUTED_TOPIC
```

### Logging Example Output

```text
[19:04:31 INF] Trade executed event with ID: 6e8b1da3-669e-4512-b309-06586d77d31c Side: Sell Quantity: 63 Price: 12.76 TotalAmount: 803.88 ExecutedAt: 05/02/2025 19:04:31
```

## Infrastructure Setup

The Kafka infrastructure is provisioned using Docker Compose:

- **Zookeeper**: Required for Kafka cluster management
- **Kafka**: Message broker running with single-node configuration
- **Kafka UI**: Web interface for topic/message inspection
