using MassTransit;
using Microsoft.Extensions.Logging;
using ProductApp.Shared.Events;
using ProductEventListener.Domain.Entities;
using ProductEventListener.Infrastructure.Persistance;
using ProductEventListener.Infrastructure.Services;

namespace ProductEventListener.Infrastructure.Consumers//domain eventleri yutar
{
    public class ProductCreatedConsumer : IConsumer<CreateProductEvent>
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

        public async Task Consume(ConsumeContext<CreateProductEvent> context)
        {
            var productId = context.Message.ProductId;
            _logger.LogInformation("Ürün ID: {ProductId} için CreateProductEvent mesajı alındı", productId);

            try
            {
                _logger.LogInformation("Ürün ID: {ProductId} için stok kontrolü yapılıyor", productId);
                var stockCheck = await _productService.CheckStockAsync(productId, context.CancellationToken);

                _logger.LogInformation("Stok kontrol sonucu - Mevcut: {Exists}, Stokta Var: {HasStock}, Stok: {Stock}",
                    stockCheck.Exists, stockCheck.HasStock, stockCheck.Stock);

                if (!stockCheck.Exists)
                {
                    var errorMessage = $"Ürün ID: {productId} bulunamadı!";
                    _logger.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                if (!stockCheck.HasStock)
                {
                    var errorMessage = $"Ürün ID: {productId} stokta yok! Mevcut stok: {stockCheck.Stock}";
                    _logger.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                _logger.LogInformation("Event log veritabanına kaydediliyor...");
                var eventLog = new ProductEventLog
                {
                    ProductId = productId,
                    OccurredOn = DateTime.UtcNow,
                };

                await _context.ProductEventLogs.AddAsync(eventLog, context.CancellationToken);
                var saved = await _context.SaveChangesAsync(context.CancellationToken);

                _logger.LogInformation("Ürün ID: {ProductId} için event log başarıyla kaydedildi. Stok: {Stock}. Etkilenen satır: {RowsAffected}",
                    productId, stockCheck.Stock, saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün ID: {ProductId} için CreateProductEvent işlenirken hata oluştu", productId);
                throw;
            }
        }
    }
}