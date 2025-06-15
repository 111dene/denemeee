using MassTransit;
using Microsoft.EntityFrameworkCore;
using MerchantNotificationService.Infrastructure.BackgroundServices;
using MerchantNotificationService.Infrastructure.Consumers;
using MerchantNotificationService.Infrastructure.Extensions;
using MerchantNotificationService.Infrastructure.Services;
using MerchantNotificationService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<INotificationProcessingService, NotificationProcessingService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Background Services
builder.Services.AddHostedService<NotificationProcessor>();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Run();