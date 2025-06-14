using MassTransit;

namespace ProductApp.Api.ServiceRegistrations;

public static class MessagingServiceRegistration//
{
    public static IServiceCollection RegisterMessagingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqSettings = configuration.GetSection("Messaging:MassTransit");

                configurator.Host(
                    rabbitMqSettings["Host"],
                    "/",
                    h =>
                    {
                        h.Username(rabbitMqSettings["Username"]);
                        h.Password(rabbitMqSettings["Password"]);
                    });
            });
        });

        return services;
    }
}