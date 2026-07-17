# RabbitMqTraining

This repository contains a simple C# example application that demonstrates how to consume and publish messages with RabbitMQ. It includes five projects that work together to provide a complete end-to-end pipeline:

| Project | Purpose | Key files |
|---------|---------|----------|
| **RabbitMqTraining.Core** | Shared interfaces and domain classes (IMessagePublisher, IOrderProcessor) | Interfaces/IOrderProcessor.cs, Interfaces/IMessagePublisher.cs |
| **RabbitMqTraining.Infrastructure** | RabbitMQ plumbing – connection factory, models, and setup logic | Messaging/*.cs, Models/RabbitMqOptions.cs |
| **RabbitMqTraining.Application** | Shared app bootstrap for console apps – wires the services | *.csproj (internal) |
| **RabbitMqTraining.Consumer** | Consumes messages, processes orders, and optionally republishes failures | Program.cs, Consumer.cs |
| **RabbitMqTraining.Publisher** | Publishes test orders to the queue | Program.cs, RabbitMqPublisher.cs |
| **RabbitMqTraining.Api** | Web API for order management and queue operations | Program.cs, Controllers/OrdersController.cs |
| **RabbitMqTraining.Worker** | Background service to process orders from the queue | Program.cs, OrderWorker.cs |

> All projects target .NET 10 with packages.config for compatibility with the learning package.

## Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* RabbitMQ instance (start with `docker compose up -d`)

## Running the Sample

1. Build everything:
   ```powershell
   dotnet build
   ```

2. Start the consumer:
   ```powershell
   dotnet run --project RabbitMqTraining.Consumer
   ```

3. Publish orders via API or Publisher:
   ```powershell
   dotnet run --project RabbitMqTraining.Api  # To run the API
   dotnet run --project RabbitMqTraining.Publisher
   ```

## New Additions

### API (RabbitMqTraining.Api)
- Provides RESTful endpoints for order management
- Key files: `OrderController.cs` for API routes, `Program.cs` for service configuration
- Access via `https://localhost:5000/api/orders` (default in launchSettings.json)

### Worker (RabbitMqTraining.Worker)
- Background service for order processing
- Key files: `OrderWorker.cs` for message handling logic, `Program.cs` for host configuration
- Automatically processes messages from the queue using the hosted service pattern

## Directory Layout

```
RabbitMqTraining/
├─ Dockerfile
├─ docker-compose.yml
├─ .gitignore
├─ README.md
├─ RabbitMqTraining.Core/            <-- Domain interfaces
├─ RabbitMqTraining.Infrastructure/  <-- RabbitMQ plumbing
├─ RabbitMqTraining.Publisher/       <-- Demo publisher
├─ RabbitMqTraining.Consumer/        <-- Demo consumer
├─ RabbitMqTraining.Application/     <-- Shared app bootstrap
├─ RabbitMqTraining.Api/              <-- New Web API
└─ RabbitMqTraining.Worker/           <-- New Background Worker
```

## Extending the Sample

* Add API endpoints in `Controllers/OrdersController.cs`
* Implement new processing logic in `OrderWorker.cs`
* Add API routes for queue statistics or health checks

## Troubleshooting

| Symptom | Fix |
|---------|-----|
| API connection issues? | Verify RabbitMQ container is running and API port (5000) is open |
| Worker not processing? | Check queue name in `RabbitMqOptions` matches consumer config |
| ... |

## License

MIT - see `LICENSE` file.