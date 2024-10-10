

using Contract.Message;
using MassTransit;
using Producer.API.DIs;
using Producer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure masstransit rabbitmq
builder.Services.AddConfigurationMasstransitRabbitMQ(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapGrpcService<MessageService>();  // Register your gRPC service here
app.MapPost("/send-message", async (IPublishEndpoint publishEndpoint, string text) =>
{
    var message = new MessageContract
    {
        Text = text
    };

    await publishEndpoint.Publish(message); // Publish message to RabbitMQ

    return Results.Ok($"Message '{text}' sent to RabbitMQ");
});
app.MapControllers();

app.Run();
