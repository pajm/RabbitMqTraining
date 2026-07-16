using RabbitMQ.Client;
using RabbitMqTraining.Infrastructure.Models;

namespace RabbitMqTraining.Infrastructure.Messaging
{
    public class RabbitMqConnectionFactory
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqConnectionFactory(RabbitMqOptions options)
        {
            _options = options;
        }

        public async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
