using MassTransit;
using MerchantNotificationService.Infrastructure.BackgroundServices;
using MerchantNotificationService.Infrastructure.Consumers;
using MerchantNotificationService.Infrastructure.Persistance;
using MerchantNotificationService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MerchantNotificationService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Services
            services.AddScoped<INotificationProcessingService, NotificationProcessingService>();
            services.AddScoped<IEmailService, EmailService>();

            // Background Services
            services.AddHostedService<NotificationProcessor>();

            // MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ReduceStockConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost");
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}