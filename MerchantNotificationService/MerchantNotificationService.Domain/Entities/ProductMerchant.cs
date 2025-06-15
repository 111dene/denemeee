namespace MerchantNotificationService.Domain.Entities;

public class ProductMerchant
{
    public Guid ProductId { get; set; }
    public Guid MerchantId { get; set; }
    public long LastNotifiedSequence { get; set; } // Son bildirim yapılan sequence
    public DateTime? LastNotifiedAt { get; set; }

    // Navigation properties
    public Merchant Merchant { get; set; }
}