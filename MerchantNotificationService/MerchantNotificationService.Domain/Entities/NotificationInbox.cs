namespace MerchantNotificationService.Domain.Entities;

public class NotificationInbox
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int RemainingStock { get; set; }
    public string OrderId { get; set; }
    public long SequenceNumber { get; set; } // Sıralama için
    public DateTime ReceivedAt { get; set; }
    public bool IsProcessed { get; set; }
    public DateTime? ProcessedAt { get; set; }
}