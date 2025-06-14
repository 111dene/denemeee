using MassTransit;
using Microsoft.Extensions.Logging;
using ProductApp.Domain.Aggregates.Product.DomainEvents;
using ProductEventListener.Domain.Entities;
using ProductEventListener.Infrastructure.Persistance;
using ProductEventListener.Infrastructure.Services;

namespace ProductEventListener.Infrastructure.Consumers
{
    public class ProductCreatedConsumer : IConsumer<AddProductEvent>
    {
        private readonly EventLogDbContext _context;
        private readonly IProductService _productService;
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(
            EventLogDbContext context,
            IProductService productService,
            ILogger<ProductCreatedConsumer> logger)
        {
            _context = context;
            _productService = productService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AddProductEvent> context)
        {
            var productId = context.Message.ProductId;
            _logger.LogInformation("Ürün ID: {ProductId} için event alındı", productId);

            try
            {
                // Stok kontrolü yap
                var stockCheck = await _productService.CheckStockAsync(productId, context.CancellationToken);

                if (!stockCheck.Exists)
                {
                    _logger.LogError("Ürün ID: {ProductId} bulunamadı", productId);
                    throw new InvalidOperationException($"Ürün ID: {productId} bulunamadı");
                }

                // Event log'u kaydet
                var eventLog = new ProductEventLog
                {
                    ProductId = productId,
                    OccurredOn = context.Message.OccurredOn
                };

                await _context.ProductEventLogs.AddAsync(eventLog, context.CancellationToken);
                await _context.SaveChangesAsync(context.CancellationToken);

                _logger.LogInformation("Ürün ID: {ProductId} için event log kaydedildi", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün ID: {ProductId} için event işlenirken hata oluştu", productId);
                throw;
            }
        }
    }
}