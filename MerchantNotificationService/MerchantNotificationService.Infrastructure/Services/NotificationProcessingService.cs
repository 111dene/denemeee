using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MerchantNotificationService.Domain.Entities;
using MerchantNotificationService.Infrastructure.Persistence;

namespace MerchantNotificationService.Infrastructure.Services;

public class NotificationProcessingService : INotificationProcessingService
{
    private readonly NotificationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationProcessingService> _logger;

    public NotificationProcessingService(
        NotificationDbContext context,
        IEmailService emailService,
        ILogger<NotificationProcessingService> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task ProcessPendingNotificationsAsync(CancellationToken cancellationToken = default)
    {
        // İşlenmemiş inbox kayıtlarını sequence sırasına göre al
        var pendingNotifications = await _context.NotificationInbox
            .Where(n => !n.IsProcessed)
            .OrderBy(n => n.SequenceNumber)
            .ToListAsync(cancellationToken);

        foreach (var notification in pendingNotifications)
        {
            await ProcessSingleNotificationAsync(notification, cancellationToken);
        }
    }

    private async Task ProcessSingleNotificationAsync(NotificationInbox notification, CancellationToken cancellationToken)
    {
        // Bu ürün için merchant'ları bul
        var productMerchants = await _context.ProductMerchants
            .Include(pm => pm.Merchant)
            .Where(pm => pm.ProductId == notification.ProductId && pm.Merchant.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var productMerchant in productMerchants)
        {
            // Sequence kontrolü - Eğer daha yeni bir bildirim gönderilmişse bu eski bildirimi atla
            if (productMerchant.LastNotifiedSequence > notification.SequenceNumber)
            {
                _logger.LogInformation(
                    "Skipping outdated notification. Product: {ProductId}, Current Sequence: {CurrentSeq}, Last Notified: {LastSeq}",
                    notification.ProductId, notification.SequenceNumber, productMerchant.LastNotifiedSequence);
                continue;
            }

            // Outbox'a mail kuyruğu ekle
            var outboxEntry = new NotificationOutbox
            {
                Id = Guid.NewGuid(),
                ProductId = notification.ProductId,
                ProductName = notification.ProductName,
                RemainingStock = notification.RemainingStock,
                MerchantEmail = productMerchant.Merchant.Email,
                MerchantName = productMerchant.Merchant.Name,
                SequenceNumber = notification.SequenceNumber,
                CreatedAt = DateTime.UtcNow,
                IsSent = false,
                RetryCount = 0
            };

            _context.NotificationOutbox.Add(outboxEntry);

            // Son bildirim sequence'ını güncelle
            productMerchant.LastNotifiedSequence = notification.SequenceNumber;
            productMerchant.LastNotifiedAt = DateTime.UtcNow;
        }

        // Inbox kaydını işlenmiş olarak işaretle
        notification.IsProcessed = true;
        notification.ProcessedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SendPendingEmailsAsync(CancellationToken cancellationToken = default)
    {
        // Gönderilmemiş mailleri sequence sırasına göre al
        var pendingEmails = await _context.NotificationOutbox
            .Where(o => !o.IsSent && (o.NextRetryAt == null || o.NextRetryAt <= DateTime.UtcNow))
            .OrderBy(o => o.SequenceNumber)
            .Take(50) // Batch olarak işle
            .ToListAsync(cancellationToken);

        foreach (var email in pendingEmails)
        {
            await SendSingleEmailAsync(email, cancellationToken);
        }
    }

    private async Task SendSingleEmailAsync(NotificationOutbox email, CancellationToken cancellationToken)
    {
        try
        {
            var subject = $"Stock Update - {email.ProductName}";
            var body = $@"
                Dear {email.MerchantName},
                
                Stock update for your product: {email.ProductName}
                Remaining Stock: {email.RemainingStock}
                
                Sequence: {email.SequenceNumber}
                Time: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
                
                Best regards,
                Stock Notification System
            ";

            await _emailService.SendEmailAsync(email.MerchantEmail, subject, body);

            email.IsSent = true;
            email.SentAt = DateTime.UtcNow;
            email.LastError = null;

            _logger.LogInformation("Email sent successfully to {Email} for Product {ProductId}",
                email.MerchantEmail, email.ProductId);
        }
        catch (Exception ex)
        {
            email.RetryCount++;
            email.LastError = ex.Message;
            email.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, email.RetryCount)); // Exponential backoff

            _logger.LogError(ex, "Failed to send email to {Email}. Retry count: {RetryCount}",
                email.MerchantEmail, email.RetryCount);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}