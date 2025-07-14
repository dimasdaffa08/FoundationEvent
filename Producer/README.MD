# Foundation Event - Producer (Protobuf Message)

A generic, reusable **Kafka Producer** implementation using `Confluent.Kafka`, `Google.Protobuf`, and structured logging, built with .NET 9 applications.

---

## Features

- Generic Kafka Producer supporting custom `TKey` and `TValue` types
- Supports `Google.Protobuf` serialization (to JSON)
- Customizable Kafka headers
- Built-in structured logging via `ILogger`
- Graceful error handling
- Follows dependency injection best practices
- Disposability pattern for clean resource management

---

## Installation

- Add the project as a reference to your solution (internal library), or package it as a NuGet if needed.

---

## Usage Example

```csharp
var headers = new Dictionary<string, string>
{
    { "key1", "val1" },
    { "key2", "val2" }
};

var message = new UserMessage
{
    UserId = "user123",
    Name = "John Doe",
    Email = "john@example.com",
    CreatedAt = DateTime.Now.ToLongDateString()
};

// Assuming `factory` is your internal abstraction that resolves the KafkaProducerImpl via DI (Dependency Injection)
var producer = factory.CreateProducer<string, UserMessage>();

var result = await producer.SendAsync("test-proto-topic", message, headers);
```

## Configuration
Add the following configuration to your `appsettings.json` or `globalsettings.json` and adjust the values as needed.

```json
{
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "ClientId": "example-app",
    "Acks": "all",
    "Retries": 3,
    "EnableIdempotence": true
  }
}
```