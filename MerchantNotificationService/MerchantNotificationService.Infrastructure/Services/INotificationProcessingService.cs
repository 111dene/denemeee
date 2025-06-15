namespace MerchantNotificationService.Infrastructure.Services;

public interface INotificationProcessingService
{
    Task ProcessPendingNotificationsAsync(CancellationToken cancellationToken);
    Task SendPendingEmailsAsync(CancellationToken cancellationToken);
}