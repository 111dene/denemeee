using MassTransit;
using Microsoft.Extensions.Logging;

using MerchantNotificationService.Domain.Entities;
using MerchantNotificationService.Infrastructure.Persistance;
using ProductApp.Domain.Aggregates.Product.DomainEvents;

namespace MerchantNotificationService.Infrastructure.Consumers;

public class ReduceStockConsumer : IConsumer<ReduceStockEvent>
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<ReduceStockConsumer> _logger;

    public ReduceStockConsumer(NotificationDbContext context, ILogger<ReduceStockConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReduceStockEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Stock reduced event received for Product: {ProductId}, Sequence: {Sequence}",
            message.ProductId, message.SequenceNumber);

        // Inbox pattern - Event'i kaydet
        var inboxEntry = new NotificationInbox
        {
            Id = Guid.NewGuid(),
            ProductId = message.ProductId,
            ProductName = message.ProductName,
            RemainingStock = message.RemainingStock,
            OrderId = message.OrderId,
            SequenceNumber = message.SequenceNumber,
            ReceivedAt = DateTime.UtcNow,
            IsProcessed = false
        };

        _context.NotificationInbox.Add(inboxEntry);
        await _context.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("Inbox entry created for ProductId: {ProductId}", message.ProductId);
    }
}