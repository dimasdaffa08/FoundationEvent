# Foundation Event - Consumer (Protobuf Message)

This project provides a reusable and configurable Kafka Consumer implementation using [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka/) in .NET, with support for dependency injection, logging, and header handling.

---

## Features

- Consumer configuration via `KafkaProperties`
- Logging and error handling via `ILogger`
- Custom header extraction
- Factory pattern for instantiating consumer
- Designed for `IHostedService` or background service usage

---

## Usage Example

Inject the factory and call the consumer:

```csharp
var consumer = _consumerFactory.CreateConsumer();

while (!stoppingToken.IsCancellationRequested)
{
    var result = await consumer.ListenAsync("test-proto-topic", stoppingToken);
}
```

---

## Configuration
Add the following configuration to your `appsettings.json` or `globalsettings.json` and adjust the values as needed.
```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "ClientId": "example-app",
    "GroupId": "test-group-1",
    "Acks": "all",
    "Retries": 3,
    "EnableIdempotence": true
  }
}
```

