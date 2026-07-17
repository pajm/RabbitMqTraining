using RabbitMQ.Client;
using RabbitMqTraining.Infrastructure.Messaging;

namespace RabbitMqTraining.Worker
{
    public class OrderWorker : BackgroundService
    {
        private readonly RabbitMqConsumer _consumer;
        private readonly RabbitMqSetup _setup;
        private readonly IConnection _connection;

        public OrderWorker(
            RabbitMqConsumer consumer,
            RabbitMqSetup setup,
            IConnection connection)
        {
            _consumer = consumer;
            _setup = setup;
            _connection = connection;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            await using var channel = await _connection.CreateChannelAsync();

            await _setup.InitialiseAsync(channel);

            await _consumer.StartAsync();

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
    }
}
