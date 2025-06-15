namespace MerchantNotificationService.Domain.Entities;

public class NotificationOutbox
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int RemainingStock { get; set; }
    public string MerchantEmail { get; set; }
    public string MerchantName { get; set; }
    public long SequenceNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
    public int RetryCount { get; set; }
    public DateTime? NextRetryAt { get; set; }
    public string? LastError { get; set; }
}