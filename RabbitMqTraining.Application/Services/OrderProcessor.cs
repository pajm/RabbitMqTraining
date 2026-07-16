using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Core.Models;

namespace RabbitMqTraining.Application.Services
{
    public class OrderProcessor : IOrderProcessor
    {
        private static readonly HashSet<Guid> ProcessedMessages = new();

        public async Task ProcessAsync(OrderCreatedEvent order)
        {

            Console.WriteLine(
                $"Processing order {order.OrderId} for {order.Customer}");

            if (ProcessedMessages.Contains(order.MessageId))
            {
                Console.WriteLine($"Duplicate message {order.MessageId} ignored.");
                return;
            }

            ProcessedMessages.Add(order.MessageId);

            Console.WriteLine($"Order {order.OrderId} processed successfully");

            //throw new Exception("Simulated failure");
        }
    }
}
