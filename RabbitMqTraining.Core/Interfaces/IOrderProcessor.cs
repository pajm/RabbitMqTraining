using RabbitMqTraining.Core.Models;

namespace RabbitMqTraining.Core.Interfaces
{
    public interface IOrderProcessor
    {
        Task ProcessAsync(OrderCreatedEvent order);
    }
}
