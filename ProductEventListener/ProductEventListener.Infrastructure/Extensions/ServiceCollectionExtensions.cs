using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductEventListener.Infrastructure.Consumers;
using ProductEventListener.Infrastructure.Persistance;
using ProductEventListener.Infrastructure.Services;

namespace ProductEventListener.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<EventLogDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // HTTP Client
            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });

            // MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                   
                });
            });

            return services;
        }
    }
}