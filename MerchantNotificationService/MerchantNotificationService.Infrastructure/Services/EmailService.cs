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
       
        await Task.Delay(100);

        // Gerçek implementasyonda SMTP/SendGrid/AWS SES kullanılır
        _logger.LogInformation("E-POSTA GÖNDERİLDİ - Kime: {To}, Konu: {Subject}", to, subject);

        // Hata simülasyonu (testing için)
        if (to.Contains("fail"))
        {
            throw new Exception("Simüle edilmiş e-posta hatası");
        }
    }
}