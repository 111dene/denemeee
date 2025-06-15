using Microsoft.Extensions.Logging;

namespace MerchantNotificationService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Simüle edilmiş email gönderimi
        await Task.Delay(100); // Network delay simulation

        // Gerçek implementasyonda SMTP/SendGrid/AWS SES kullanılır
        _logger.LogInformation("📧 EMAIL SENT - To: {To}, Subject: {Subject}", to, subject);

        // Hata simülasyonu (testing için)
        if (to.Contains("fail"))
        {
            throw new Exception("Simulated email failure");
        }
    }
}