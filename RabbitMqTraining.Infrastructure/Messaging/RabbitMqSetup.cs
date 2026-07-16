using RabbitMQ.Client;
using RabbitMqTraining.Infrastructure.Models;

namespace RabbitMqTraining.Infrastructure.Messaging
{
    public class RabbitMqSetup
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqSetup(RabbitMqOptions options)
        {
            _options = options;
        }


        public async Task InitialiseAsync(IChannel channel)
        {
            await CreateMainQueueAsync(channel);

            await CreateRetryQueueAsync(channel);

            await CreateDeadLetterQueueAsync(channel);
        }

        private async Task CreateMainQueueAsync(IChannel channel)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            await channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: _options.QueueName,
                exchange: _options.ExchangeName,
                routingKey: _options.RoutingKey);
        }

        private async Task CreateRetryQueueAsync(IChannel channel)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _options.RetryExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            var arguments = new Dictionary<string, object>
                {
                    { "x-message-ttl", _options.RetryDelayMilliseconds },
                    { "x-dead-letter-exchange", _options.ExchangeName },
                    { "x-dead-letter-routing-key", _options.RoutingKey }
                };

            await channel.QueueDeclareAsync(
                queue: _options.RetryQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments);

            await channel.QueueBindAsync(
                queue: _options.RetryQueueName,
                exchange: _options.RetryExchangeName,
                routingKey: _options.RoutingKey);
        }

        private async Task CreateDeadLetterQueueAsync(IChannel channel)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _options.DeadLetterExchangeName,
                type: ExchangeType.Direct,
                durable: true);


            await channel.QueueDeclareAsync(
                queue: _options.DeadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);


            await channel.QueueBindAsync(
                queue: _options.DeadLetterQueueName,
                exchange: _options.DeadLetterExchangeName,
                routingKey: _options.RoutingKey);
        }
    }
}
