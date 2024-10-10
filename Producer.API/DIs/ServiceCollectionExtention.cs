using MassTransit;

namespace Producer.API.DIs
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddConfigurationMasstransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var mtsConfig = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(mtsConfig);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(mtsConfig.Host, mtsConfig.VHost, h =>
                    {
                        h.Username(mtsConfig.UserName);
                        h.Password(mtsConfig.Password);
                    });
                });
            });
            return services;
        }
    }
}
