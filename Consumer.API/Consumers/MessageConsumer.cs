using Contract.Message;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Consumer.API.Consumers
{
    public class MessageConsumer : IConsumer<MessageContract> // Implement IConsumer<MessageContract>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MessageContract> context) // Implement the Consume method
        {
            _logger.LogInformation("Received message: {Text}", context.Message.Text);
            return Task.CompletedTask; // Simply log the message for now
        }
    }
}
