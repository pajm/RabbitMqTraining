using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Core.Models;
using RabbitMqTraining.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace RabbitMqTraining.Infrastructure.Messaging
{
    public class RabbitMqConsumer
    {
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;
        private readonly IOrderProcessor _processor;
        private readonly IMessagePublisher _publisher;


        public RabbitMqConsumer(
            IConnection connection,
            RabbitMqOptions options,
            IOrderProcessor processor,
            IMessagePublisher publisher)
        {
            _connection = connection;
            _options = options;
            _processor = processor;
            _publisher = publisher;
        }

        public async Task StartAsync()
        {
            var channel = await _connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                OrderCreatedEvent? order = null;

                try
                {
                    var body = args.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    order = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                    await _processor.ProcessAsync(order!);

                    await channel.BasicAckAsync(
                        args.DeliveryTag,
                        false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    if (order == null)
                    {
                        await channel.BasicNackAsync(
                            args.DeliveryTag,
                            false,
                            false);

                        return;
                    }

                    order.RetryCount++;

                    Console.WriteLine($"Retry count: {order.RetryCount}");

                    if (order.RetryCount >= _options.MaxRetryAttempts)
                    {
                        Console.WriteLine("Maximum retries reached. Sending to dead letter queue.");

                        await _publisher.PublishAsync(
                            order,
                            _options.DeadLetterExchangeName,
                            _options.RoutingKey);
                    }
                    else
                    {
                        Console.WriteLine("Sending to retry queue.");

                        await _publisher.PublishAsync(
                            order,
                            _options.RetryExchangeName,
                            _options.RoutingKey);
                    }

                    await channel.BasicAckAsync(
                        args.DeliveryTag,
                        false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: _options.QueueName,
                autoAck: false,
                consumer: consumer);
        }

    }
}
