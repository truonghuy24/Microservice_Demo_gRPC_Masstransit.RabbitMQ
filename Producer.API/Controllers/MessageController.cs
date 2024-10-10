using Contract.Message;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Producer.API.Protos;

namespace Producer.API.Controllers
{
    [ApiController]
    [Route("message")]
    public class MessageController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IPublishEndpoint publishEndpoint, ILogger<MessageController> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Message text cannot be empty.");
            }

            // Publish message to RabbitMQ
            var message = new MessageContract
            {
                Text = request.Text
            };

            await _publishEndpoint.Publish(message);

            _logger.LogInformation($"Message '{request.Text}' sent to RabbitMQ");

            // Prepare and return the HTTP response
            return Ok(new
            {
                Confirmation = $"Message '{request.Text}' sent to RabbitMQ"
            });
        }
    }
}
