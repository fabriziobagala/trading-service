# 🏦 Trading Service

A microservice-based trading platform built using **ASP.NET 8** and designed according to **Clean Architecture** principles.

## 📋 Table of Contents

- Overview
- Features
- Architecture
- Tech Stack
- Getting Started
- Configuration
- SSL Setup
- Testing
- Project Structure
- Documentation
- License

## 📖 Overview

**Trading Service** is a scalable microservice for executing and managing financial trades with:

* **RESTful API** for trade operations with OpenAPI documentation
* **Event-Driven Architecture** using Kafka for asynchronous processing
* **Clean Domain Model** with proper separation of concerns
* **CQRS pattern** for optimized read/write operations
* **Docker containerization** for consistent deployment

The service demonstrates modern software architecture principles in a financial domain context, providing both synchronous API access and asynchronous event processing capabilities.

## ✨ Features

* **Domain-Driven Design** with Clean Architecture layers
* **CQRS implementation** using MediatR for command/query separation
* **Event Sourcing** with Kafka for reliable trade event processing
* **Distributed Caching** via Redis for performance optimization
* **Interactive API documentation** with Swagger/OpenAPI
* **Containerized Infrastructure** with Docker Compose
* **Structured Logging** with Serilog and centralized in Seq
* **Test Coverage** with unit tests and mocking

## 🏗️ Architecture

The solution follows Clean Architecture with these layers:

* **API Layer** (`TradingService.API`): Controllers, middleware, API documentation
* **Application Layer** (`TradingService.Application`): Business logic, CQRS handlers, validators
* **Domain Layer** (`TradingService.Domain`): Core entities and business rules
* **Infrastructure Layer** (`TradingService.Infrastructure`): Data persistence, messaging, caching
* **Consumer** (`TradingService.Consumer`): Kafka event consumer service
* **Shared Layer** (`TradingService.Shared`): DTOs, common utilities, and shared models

<div align="center">
  <img src="https://blog.ndepend.com/wp-content/uploads/Clean-Architecture-Diagram-Asp-Net.png" alt="Clean Architecture Diagram" width="600" />
  <p><small>Image credit: <a href="https://blog.ndepend.com">NDepend Blog</a></small></p>
</div>

## 🧰 Tech Stack

| Category         | Technology              | Purpose                             |
|------------------|-------------------------|------------------------------------|
| Framework        | ASP.NET Core 8          | Web API and application framework   |
| API Docs         | Swagger / OpenAPI       | Interactive API documentation       |
| Messaging        | Apache Kafka            | Event streaming and processing      |
| Database         | PostgreSQL              | Persistent data storage             |
| Caching          | Redis                   | Distributed caching                 |
| Logging          | Serilog + Seq           | Structured logging and visualization|
| Containerization | Docker & Docker Compose | Environment consistency             |
| ORM              | Entity Framework Core   | Data access and migrations          |
| Patterns         | CQRS, Repository        | Architectural design patterns       |
| Validation       | FluentValidation        | Request validation                  |

## 🚀 Getting Started

### 🔧 Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Docker Compose](https://docs.docker.com/compose/)
* [Visual Studio Code](https://code.visualstudio.com/) (recommended)

### 📦 Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/fabriziobagala/trading-service.git
   cd trading-service
   ```

2. **Start the infrastructure and services**
   ```bash
   cd docker
   docker-compose up -d
   ```

3. **Access the API**
   * HTTP: [http://localhost:8080/swagger](http://localhost:8080/swagger)
   * HTTPS: [https://localhost:8081/swagger](https://localhost:8081/swagger)
   * Seq Logs: [http://localhost:5341](http://localhost:5341)
   * Kafka UI: [http://localhost:8090](http://localhost:8090)

## ⚙️ Configuration

All configuration is centralized in the `.env` file within the docker directory:

### Application Settings

```bash
# ASP.NET Core configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:80;https://+:443
API_HTTP_PORT=8080
API_HTTPS_PORT=8081

# SSL configuration
SSL_CERT_PATH=/app/devcert.pfx
SSL_CERT_PASSWORD=password
SSL_CERT_HOST_PATH=../src/TradingService.API/devcert.pfx

# .NET Core configuration
DOTNET_ENVIRONMENT=Development
```

### Data Storage

```bash
# PostgreSQL configuration
POSTGRES_PORT=5432
POSTGRES_USER=username
POSTGRES_PASSWORD=password
POSTGRES_DB=db_name
DB_CONNECTION_STRING=Host=postgres;Database=db_name;Username=usernam;Password=password

# Redis configuration
REDIS_PORT=6379
REDIS_CONNECTION_STRING=redis:6379
REDIS_INSTANCE_NAME=istance:
```

### Messaging

```bash
# Zookeeper configuration
ZOOKEEPER_CLIENT_PORT=2181
ZOOKEEPER_TICK_TIME=2000
ZOOKEEPER_CONNECT=zookeeper:2181

# Kafka configuration
KAFKA_PORT=9092
KAFKA_HOST_PORT=9093
KAFKA_BOOTSTRAP_SERVERS=kafka:9092
KAFKA_BROKER_ID=1
KAFKA_GROUP_ID=trading-consumer-group
KAFKA_TRADE_EXECUTED_TOPIC=trade-executed
KAFKA_AUTO_OFFSET_RESET=earliest
KAFKA_ALLOW_AUTO_CREATE_TOPICS=true
KAFKA_LISTENERS=PLAINTEXT://:9092,PLAINTEXT_HOST://:9093
KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://kafka:9092,PLAINTEXT_HOST://localhost:9093
KAFKA_LISTENER_SECURITY_PROTOCOL_MAP=PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
KAFKA_INTER_BROKER_LISTENER_NAME=PLAINTEXT
KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1
KAFKA_TRANSACTION_STATE_LOG_MIN_ISR=1
KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR=1
KAFKA_AUTO_CREATE_TOPICS_ENABLE=true
KAFKA_HEALTHCHECK_INTERVAL=10s
KAFKA_HEALTHCHECK_TIMEOUT=5s
KAFKA_HEALTHCHECK_RETRIES=5

# Kafka UI configuration
KAFKA_UI_PORT=8090
KAFKA_UI_CLUSTER_NAME=trading-service
```

### Logging

```bash
# Seq configuration
SEQ_API_PORT=5341
SEQ_UI_PORT=80
SEQ_SERVER_URL=http://seq:5341
```

## 🔐 SSL Certificate Setup (Development)

For local HTTPS development:

1. **Generate a development certificate**:
   ```bash
   dotnet dev-certs https -ep src/TradingService.API/devcert.pfx -p password
   ```

2. **Configure environment variables** in `.env`:
   ```env
   SSL_CERT_PATH=/app/devcert.pfx
   SSL_CERT_PASSWORD=password
   SSL_CERT_HOST_PATH=../src/TradingService.API/devcert.pfx
   ```

3. Docker Compose mounts this certificate into the API container for HTTPS access.

> ⚠️ **Warning**: For production, replace with a certificate from a trusted Certificate Authority.

## 🧪 Testing

The solution includes comprehensive unit tests:

```bash
cd tests/TradingService.UnitTests
dotnet test
```

**Testing principles**:
* Behavior-driven unit tests with Moq for isolation
* Test-driven development for core business logic
* Separation of infrastructure concerns for testability
* Focused unit tests for CQRS handlers and domain logic

## 📁 Project Structure

```
trading-service/
├── src/
│   ├── TradingService.API/            # Controllers, middleware, API config
│   ├── TradingService.Application/    # Commands, queries, validators
│   ├── TradingService.Consumer/       # Kafka consumer service
│   ├── TradingService.Domain/         # Entities, value objects, interfaces
│   ├── TradingService.Infrastructure/ # Data access, messaging, caching
│   └── TradingService.Shared/         # DTOs, utilities, constants
├── tests/
│   └── TradingService.UnitTests/      # Unit tests for all components
├── docker/                            # Containerization configs
│   ├── .env                           # Environment variables
│   ├── docker-compose.yml             # Service definitions
│   ├── Dockerfile.api                 # API service image
│   └── Dockerfile.consumer            # Consumer service image
└── docs/                              # Technical documentation
    ├── api.md                         # API endpoints documentation
    ├── docker-compose.md              # Infrastructure details
    └── kafka.md                       # Event messaging documentation
```

## 📚 Documentation

Detailed technical documentation is available in the [docs](docs) directory:

* [API Documentation](docs/api.md) - Endpoints, request/response formats, status codes
* [Kafka and Messaging](docs/kafka.md) - Event schema, producer/consumer implementation
* [Docker Compose](docs/docker-compose.md) - Infrastructure components and configuration

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
