

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
app.MapGet("/", () => "Use a gRPC client to communicate with gRPC endpoints.");

app.MapControllers();

app.Run();
