# Docker Compose Documentation

This document provides in-depth explanations of each component in the Docker Compose configuration.

## TradingService API Configuration

```
tradingservice-api:
  build:
    context: ..
    dockerfile: docker/Dockerfile.api
  ports:
    - "${API_HTTP_PORT}:80"
    - "${API_HTTPS_PORT}:443"
  depends_on:
    - postgres
    - redis
    - kafka
  environment:
    - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
    - ASPNETCORE_URLS=${ASPNETCORE_URLS}
    - ASPNETCORE_Kestrel__Certificates__Default__Path=${SSL_CERT_PATH}
    - ASPNETCORE_Kestrel__Certificates__Default__Password=${SSL_CERT_PASSWORD}
    - ConnectionStrings__DefaultConnection=${DB_CONNECTION_STRING}
    - Redis__ConnectionString=${REDIS_CONNECTION_STRING}
    - Redis__InstanceName=${REDIS_INSTANCE_NAME}
    - Kafka__BootstrapServers=${KAFKA_BOOTSTRAP_SERVERS}
    - Kafka__TradeExecutedTopic=${KAFKA_TRADE_EXECUTED_TOPIC}
    - Seq__ServerUrl=${SEQ_SERVER_URL}
  volumes:
    - ${SSL_CERT_HOST_PATH}:/app/devcert.pfx:ro
  networks:
    - trading-network
```

Key components:

- `build`: Custom build from project source
  - `context`: Parent directory containing source code
  - `dockerfile`: Uses `docker/Dockerfile.api` for build instructions
- `ports`: Maps container ports to host
  - `80`: HTTP port mapped to configurable host port
  - `443`: HTTPS port mapped to configurable host port
- `depends_on`: Service dependencies
  - Ensures database, cache, and message broker are running
- `environment`: Configuration through environment variables
  - ASP.NET Core configuration (environment, URLs, HTTPS cert)
  - Connection strings for PostgreSQL and Redis
  - Kafka configuration for event publishing
  - Seq logging endpoint
- `volumes`: Mount SSL certificate for HTTPS support
  - Read-only mount of development certificate
- `networks`: Connects to the `trading-network` for inter-service communication

## TradingService Consumer Configuration

```
tradingservice-consumer:
  build:
    context: ..
    dockerfile: docker/Dockerfile.consumer
  depends_on:
    kafka:
      condition: service_healthy
    seq:
      condition: service_started
  environment:
    - DOTNET_ENVIRONMENT=${DOTNET_ENVIRONMENT}
    - Kafka__BootstrapServers=${KAFKA_BOOTSTRAP_SERVERS}
    - Kafka__GroupId=${KAFKA_GROUP_ID}
    - Kafka__AutoOffsetReset=${KAFKA_AUTO_OFFSET_RESET}
    - Kafka__AllowAutoCreateTopics=${KAFKA_ALLOW_AUTO_CREATE_TOPICS}
    - Kafka__TradeExecutedTopic=${KAFKA_TRADE_EXECUTED_TOPIC}
    - Seq__ServerUrl=${SEQ_SERVER_URL}
  networks:
    - trading-network
```

Key components:

- `build`: Custom build from project source
  - `context`: Parent directory containing source code
  - `dockerfile`: Uses `docker/Dockerfile.consumer` for build instructions
- `depends_on`: Service dependencies with conditions
  - Waits for Kafka to be healthy before starting (using healthcheck)
  - Ensures Seq is available for logging
- `environment`: Configuration through environment variables
  - .NET environment setting
  - Kafka consumer configuration (bootstrap servers, group ID)
  - Topic subscription and offset behavior
  - Seq logging endpoint
- `networks`: Connects to the `trading-network` for inter-service communication


## PostgreSQL Configuration

```
postgres:
  image: postgres:17.4
  ports:
    - "${POSTGRES_PORT}:5432"
  environment:
    - POSTGRES_USER=${POSTGRES_USER}
    - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
    - POSTGRES_DB=${POSTGRES_DB}
  volumes:
    - postgres-data:/var/lib/postgresql/data
  networks:
    - trading-network
```

Key components:

- `image`: Official PostgreSQL 17.4 image
- `ports`: Maps container ports to host
  - `5432`: HTTP port mapped to configurable host port
- `environment`: Configuration through environment variables
  - `POSTGRES_USER`: Database username
  - `POSTGRES_PASSWORD`: Database password
  - `POSTGRES_DB`: Database name
- `volumes`: Persistent storage for database files
  - `postgres-data:/var/lib/postgresql/data`: Ensures data survives container restarts
- `networks`: Connects to the `trading-network` for inter-service communication

## Redis Configuration

```
redis:
  image: redis:7.4
  ports:
    - "${REDIS_PORT}:6379"
  volumes:
    - redis-data:/data
  networks:
    - trading-network
```

Key components:

- `image`: Official Redis 7.4 image
- `ports`: Maps container ports to host
  - `6379`: HTTP port mapped to configurable host port
- `volumes`: Persistent storage for Redis data
  - `redis-data:/data`: Ensures data survives container restarts
- `networks`: Connects to the `trading-network` for inter-service communication

## Zookeper Configuration

```
zookeeper:
  image: confluentinc/cp-zookeeper:7.9.0
  environment:
    ZOOKEEPER_CLIENT_PORT: ${ZOOKEEPER_CLIENT_PORT}
    ZOOKEEPER_TICK_TIME: ${ZOOKEEPER_TICK_TIME}
  networks:
    - trading-network
```

Key components:

- `image`: Confluent's ZooKeeper 7.9.0 image
- `environment`: Configuration through environment variables
  - `ZOOKEEPER_CLIENT_PORT`: Port for client connections
  - `ZOOKEEPER_TICK_TIME`: Time unit in milliseconds
- `networks`: Connects to the `trading-network` for inter-service communication

## Kafka Configuration

```
kafka:
  image: confluentinc/cp-kafka:7.9.0
  depends_on:
    - zookeeper
  ports:
    - "${KAFKA_PORT}:9092"
    - "${KAFKA_HOST_PORT}:9093"
  environment:
    KAFKA_BROKER_ID: ${KAFKA_BROKER_ID}
    KAFKA_ZOOKEEPER_CONNECT: ${ZOOKEEPER_CONNECT}
    KAFKA_LISTENERS: ${KAFKA_LISTENERS}
    KAFKA_ADVERTISED_LISTENERS: ${KAFKA_ADVERTISED_LISTENERS}
    KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: ${KAFKA_LISTENER_SECURITY_PROTOCOL_MAP}
    KAFKA_INTER_BROKER_LISTENER_NAME: ${KAFKA_INTER_BROKER_LISTENER_NAME}
    KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: ${KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR}
    KAFKA_TRANSACTION_STATE_LOG_MIN_ISR: ${KAFKA_TRANSACTION_STATE_LOG_MIN_ISR}
    KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR: ${KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR}
    KAFKA_AUTO_CREATE_TOPICS_ENABLE: ${KAFKA_AUTO_CREATE_TOPICS_ENABLE}
  networks:
    - trading-network
  healthcheck:
    test: ["CMD-SHELL", "kafka-topics --bootstrap-server kafka:9092 --list"]
    interval: ${KAFKA_HEALTHCHECK_INTERVAL}
    timeout: ${KAFKA_HEALTHCHECK_TIMEOUT}
    retries: ${KAFKA_HEALTHCHECK_RETRIES}
```

Key components:

- `image`: Confluent's Kafka 7.9.0 image
- `ports`: Maps container ports to host
    - `9092`: Used for internal container communication
    - `9093`: Exposed to host machine for external clients
- `environment`: Configuration through environment variables
    - `KAFKA_BROKER_ID`: Unique identifier for this Kafka broker
    - `KAFKA_LISTENERS`: Socket endpoints where Kafka will listen for connections
    - `KAFKA_ADVERTISED_LISTENERS`: How clients should connect to Kafka
        - `PLAINTEXT`: For container-to-container communication
        - `PLAINTEXT_HOST`: For host-to-container communication
    - `KAFKA_LISTENER_SECURITY_PROTOCOL_MAP`: Maps listener names to security protocols
    - `KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR`: Set to 1 for single-node development
    - `KAFKA_AUTO_CREATE_TOPICS_ENABLE`: Allows automatic topic creation
- `networks`: Connects to the `trading-network` for inter-service communication
- `healthcheck`: Ensures Kafka is fully operational before dependent services start
    - Runs kafka-topics command to verify readiness
    - Critical for the `tradingservice-consumer` which depends on Kafka being available

## Kafka UI Configuration

```
kafka-ui:
  image: provectuslabs/kafka-ui:latest
  ports:
    - "${KAFKA_UI_PORT}:8080"
  environment:
    - KAFKA_CLUSTERS_0_NAME=${KAFKA_UI_CLUSTER_NAME}
    - KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=${KAFKA_BOOTSTRAP_SERVERS}
    - KAFKA_CLUSTERS_0_ZOOKEEPER=${ZOOKEEPER_CONNECT}
  depends_on:
    - kafka
  networks:
    - trading-network
```

Key components:

- `image`: Latest Provectus Kafka UI image
- `ports`: Maps container ports to host
  - `8080`: HTTP port mapped to configurable host port
- `environment`: Configuration through environment variables
  - `KAFKA_CLUSTERS_0_NAME`: Display name for the Kafka cluster
  - `KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS`: Connection to Kafka broker
  - `KAFKA_CLUSTERS_0_ZOOKEEPER`: Connection to ZooKeeper
- `depends_on`: Ensures Kafka is running before starting the UI
- `networks`: Connects to the `trading-network` for inter-service communication

## Seq Configuration

```
seq:
  image: datalust/seq:latest
  ports:
    - "${SEQ_API_PORT}:5341"  # Entry point
    - "${SEQ_UI_PORT}:80"     # Web UI
  environment:
    - ACCEPT_EULA=Y
  networks:
    - trading-network
```

Key components:

- `image`: Latest Datalust's Seq image
- `ports`: Maps container ports to host
  - `5341`: API endpoint for log ingestion
  - `80`: Web UI mapped to configurable port
- `environment`: Configuration through environment variables
  - `ACCEPT_EULA=Y`: Required to start Seq
- `networks`: Connects to the `trading-network` for inter-service communication

## Networking

```
networks:
  trading-network:
    driver: bridge
```

Key components:

- **Bridge Network**: Isolates containers while enabling communication
- **Shared Network**: All services are on the same network
- **Hostname Resolution**: Services can reference each other by container name
  - Example: "kafka:9092" rather than IP addresses
- **Internal DNS**: Automatic service discovery within the network

## Volumes

```
volumes:
  postgres-data:
  redis-data:
```

Key components:

- **Named Volumes**: Managed by Docker
- `postgres-data`: Persistent storage for PostgreSQL database
- `redis-data`: Persistent storage for Redis cache
- **Benefits**:
  - Data survives container restarts
  - Independent of container lifecycle
  - Easily backed up or migrated
  - Better performance than bind mounts
