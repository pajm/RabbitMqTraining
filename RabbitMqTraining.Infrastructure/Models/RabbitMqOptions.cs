namespace RabbitMqTraining.Infrastructure.Models
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = "localhost";

        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";


        // Main queue
        public string ExchangeName { get; set; } = "orders.exchange";

        public string QueueName { get; set; } = "orders";

        public string RoutingKey { get; set; } = "order.created";


        // Retry queue
        public string RetryExchangeName { get; set; } = "orders.retry.exchange";

        public string RetryQueueName { get; set; } = "orders.retry";

        public int RetryDelayMilliseconds { get; set; } = 30000;

        public string DeadLetterExchangeName { get; set; } = "orders.dead.exchange";

        public string DeadLetterQueueName { get; set; } = "orders.dead";

        public int MaxRetryAttempts { get; set; } = 3;
    }
}
