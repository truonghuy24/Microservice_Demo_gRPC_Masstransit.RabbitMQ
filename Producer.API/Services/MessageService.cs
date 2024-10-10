using Contract.Message;
using Grpc.Core;
using MassTransit;
using Producer.API.Protos;

namespace Producer.API.Services;

public class MessageService : Messages.MessagesBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public override async Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
    {
        // Publish message to RabbitMQ
        var message = new MessageContract
        {
            Text = request.Text
        };

        await _publishEndpoint.Publish(message);

        // Prepare and return the gRPC response
        var reply = new MessageReply
        {
            Confirmation = $"Message '{request.Text}' sent to RabbitMQ"
        };

        return reply;
    }
}