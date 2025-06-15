using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MerchantNotificationService.Infrastructure.Services;

namespace MerchantNotificationService.Infrastructure.BackgroundServices;

public class NotificationProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationProcessor> _logger;

    public NotificationProcessor(IServiceProvider serviceProvider, ILogger<NotificationProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationProcessingService>();

                // 1. Inbox'taki event'leri işle
                await notificationService.ProcessPendingNotificationsAsync(stoppingToken);

                // 2. Outbox'taki mailleri gönder
                await notificationService.SendPendingEmailsAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // 5 saniyede bir çalış
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in notification processing");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Hata durumunda daha uzun bekle
            }
        }
    }
}