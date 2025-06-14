using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductEventListener.Infrastructure.Consumers;
using ProductEventListener.Infrastructure.Persistance;
using ProductEventListener.Infrastructure.Services;

namespace ProductEventListener.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<EventLogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // HTTP Client with certificate bypass for development
        services.AddHttpClient<IProductService, ProductService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        });
        //http client ile product apiye bağlanır ve stok kontrolü yapılır

        // MassTransit configuration
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });



                cfg.ReceiveEndpoint("product-event-listener-queue", e =>
                {
                    e.ConfigureConsumer<ProductCreatedConsumer>(context);

                    
                    e.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

                });
            });
        });

        return services;
    }
}