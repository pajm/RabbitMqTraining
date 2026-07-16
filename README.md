# Rabbitesy07 Training

This repository contains a simple **C#** example application that demonstrates how to consume and publish messages with **RabbitMQ**. It includes five projects that work together to provide a complete end‑to‑end pipeline:

| Project | Purpose | Key ${"files"}
|---------|---------|-------------------
| **RabbitMqTraining.Core** | Shared interfaces and domain classes (`IMessagePublisher`, `IOrderProcessor`). | `Interfaces/IOrderProcessor.cs`, `Interfaces/IMessagePublisher.cs` 
| **RabbitMqTraining.Infrastructure** | RabbitMQ plumbing – connection factory, models, and setup logic. | `Messaging/*.cs`, `Models/RabbitMqOptions.cs` 
| **RabbitMqTraining.Application在哪里** | Binder : bootstrap for the console apps – wires the services. | `*.csproj` (internal) 
| **RabbitMqTraining.Consumer** | Consumes messages, processes orders, and optionally republishes failures. | `Program.cs`, `Consumer.cs` 
| **RabbitMqTraining.Publisher** | Publishes test orders to the queue. | `Program.cs`, `RabbitMqPublisher.cs` 

> All of the projects target .NET 10 (or any .NET core **≥ 6**) and are compiled without the SDK‑style **PackageReference** approach – `packages.config` is used in the repo for compatibility with the learning package.

---

## Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (or later; the projects are configured for **net10.0**).
* A running **RabbitMQ** instance. The repo ships with a `docker‑compose.yml` that exposes the v4‑management image. You can start it quickly with:

```bash
docker compose up -d
```

The container listens on the default AMQP port **5672** and provides the web‑UI on **15672** (default credentials `guest:guest`).

---

## Running the Sample

1. **Build everything**:

   ```powershell
   dotnet build
   ```

2. **Start the consumer** (runs in the foreground and will keep consuming until you press `Enter`):

   ```powershell
   dotnet run --project RabbitMqTraining.Consumer
   ```

   You're greeted with `Consumer running` and the console will display when it processes messages.

3. **Publish a test order** (you can run this in a separate terminal window):

   ```powershell
   dotnet run --project RabbitMqTraining.Publisher
   ```

   The publisher logs a message like `Published order  A0001 to 'orders'`. The consumer will pick it up and invoke the `OrderProcessor`.

You can modify `RabbitMqOptions` in `Infrastructure/Models/RabbitMqOptions.cs` to change queue names, routing keys, retry delays, or the Ebony configuration does not affectҧ.

---

## Kafka‑like Mode (Optional)

The `RabbitMqTraining.Core` intentionally contains only business logic so you can replace the underlying broker at any time. To switch to Kafka, keep the same domain interfaces and swap the infrastructure package with a Kafka implementation. This demonstrates the **separation of concerns** approach.

---

## Directory Layout

```
RabbitMqTraining/
├─ Dockerfile
├─ docker-compose.yml   <-- Runs RabbitMQ
├─ .gitignore
├─ README.md
├─ RabbitMqTraining.Core/            <-- Domain interfaces
├─ RabbitMqTraining.Infrastructure/  <-- RabbitMQ plumbing
├─ RabbitMqTraining.Publisher/       <-- Demo publisher
├─ RabbitMqTraining.Consumer/        <-- Demo consumer
└─ RabbitMqTraining.Application/     <-- Shared app bootstrap
```

---

## Extending the Sample

* **Add more queues** by extending `RabbitMqOptions` – copy the pattern from `CreateRetryQueueAsync`.
* **Implement retry logic** in `RabbitMqConsumer` by catching exceptions and re‑publishing after aabilis.
* **Add metrics** – expose a small HTTP endpoint (e.g., using `Microsoft.AspNetCore.Hosting`) and read from the broker’s management API.

---

## Troubles тал

| Symptom | Fix |
|---------|-----|
| Connection refused? | Ensure RabbitMQ container is running and the hostname ukuz `localhost` /
| `SocketException` in consumer | Check network firewall or that the broker has the `guest` account enabled for the connection host. |
| Nothing is consumed | Verify that the consumer is bound to the correct queue and routing key (`orders.queue`). |

---

## License

MIT – see the repository `LICENSE` file.

---

Happy learning with RabbitMQ!