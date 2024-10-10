using Consumer.API.Consumers;
using MassTransit;

namespace Consumer.API.DIs
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddConfigurationMasstransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var mtsConfig = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(mtsConfig);

            services.AddMassTransit(mt =>
            {
                mt.AddConsumer<MessageConsumer>(); // Register the consumer

                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(mtsConfig.Host, mtsConfig.VHost, h =>
                    {
                        h.Username(mtsConfig.UserName);
                        h.Password(mtsConfig.Password);
                    });

                    // Define the receive endpoint for your consumer
                    cfg.ReceiveEndpoint("message-queue", ep =>
                    {
                        ep.ConfigureConsumer<MessageConsumer>(context); // Configure the consumer
                    });
                });
            });

            return services;
        }
    }
}
