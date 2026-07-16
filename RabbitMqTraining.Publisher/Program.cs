using RabbitMqTraining.Core.Models;
using RabbitMqTraining.Infrastructure.Messaging;
using RabbitMqTraining.Infrastructure.Models;

var options = new RabbitMqOptions();

var connectionFactory = new RabbitMqConnectionFactory(options);

var connection = await connectionFactory.CreateConnectionAsync();

var publisher = new RabbitMqPublisher(connection, options);

var random = new Random();

var customers = new[]
{
    "Patrick",
    "Alice",
    "Bob",
    "Charlie",
    "David",
    "Emma",
    "Sarah",
    "Tom"
};

var previousOrders = new List<OrderCreatedEvent>();

var orderNumber = 1001;

while (true)
{
    OrderCreatedEvent order;

    // 20% chance of publishing a duplicate
    if (previousOrders.Any() && random.Next(1, 6) == 1)
    {
        order = previousOrders[random.Next(previousOrders.Count)];

        Console.WriteLine(
            $"Publishing DUPLICATE Order {order.OrderId} ({order.Customer})");
    }
    else
    {
        order = new OrderCreatedEvent
        {
            MessageId = Guid.NewGuid(),
            OrderId = orderNumber++,
            Customer = customers[random.Next(customers.Length)],
            Amount = Math.Round(
                (decimal)(random.NextDouble() * 200 + 10),
                2)
        };

        previousOrders.Add(order);

        Console.WriteLine(
            $"Publishing Order {order.OrderId} ({order.Customer})");
    }

    await publisher.PublishAsync(
        order,
        options.ExchangeName,
        options.RoutingKey);

    var delay = random.Next(1000, 5001);

    Console.WriteLine($"Waiting {delay}ms...\n");

    await Task.Delay(delay);
}