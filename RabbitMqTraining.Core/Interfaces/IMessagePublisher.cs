namespace RabbitMqTraining.Core.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(
            T message,
            string exchange,
            string routingKey);
    }
}
