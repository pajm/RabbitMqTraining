using RabbitMQ.Client;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Infrastructure.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace RabbitMqTraining.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;
        public RabbitMqPublisher(
            IConnection connection,
            RabbitMqOptions options)
        {
            _connection = connection;
            _options = options;
        }

        public async Task PublishAsync<T>(T message,
                string exchange,
                string routingKey)
        {
            await using var channel = await _connection.CreateChannelAsync();

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: body);
        }
    }
}
