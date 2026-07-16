namespace RabbitMqTraining.Core.Models
{
    public class OrderCreatedEvent
    {
        public Guid MessageId { get; set; }

        public int OrderId { get; set; }

        public string Customer { get; set; } = "";

        public decimal Amount { get; set; }
        public int RetryCount { get; set; }
    }
}
