using Microsoft.AspNetCore.Mvc;
using RabbitMqTraining.Core.Interfaces;
using RabbitMqTraining.Core.Models;

namespace RabbitMqTraining.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMessagePublisher _publisher;

        public OrdersController(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }


        [HttpPost]
        public async Task<IActionResult> Create(OrderCreatedEvent order)
        {
            order.MessageId = Guid.NewGuid();

            await _publisher.PublishAsync(
                order,
                "orders.exchange",
                "order.created");

            return Accepted(order);
        }
    }
}
