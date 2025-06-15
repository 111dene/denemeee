using MassTransit;
using Microsoft.EntityFrameworkCore;
using MerchantNotificationService.Infrastructure.BackgroundServices;
using MerchantNotificationService.Infrastructure.Consumers;
using MerchantNotificationService.Infrastructure.Persistence;
using MerchantNotificationService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<INotificationProcessingService, NotificationProcessingService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Background Services
builder.Services.AddHostedService<NotificationProcessor>();

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockReducedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("merchant-notification-queue", e =>
        {
            e.ConfigureConsumer<StockReducedConsumer>(context);
            e.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
        });
    });
});

var app = builder.Build();

app.Run();