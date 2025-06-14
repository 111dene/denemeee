using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ProductEventListener.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private readonly string _productApiBaseUrl;

        public ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _productApiBaseUrl = configuration["ProductApi:BaseUrl"] ?? "https://localhost:7138";
        }

        public async Task<ProductStockCheckResult> CheckStockAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_productApiBaseUrl}/api/Products/CheckStock/{productId}",
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Stok kontrolü başarısız. ProductId: {ProductId}, StatusCode: {StatusCode}",
                        productId, response.StatusCode);
                    return new ProductStockCheckResult { ProductId = productId, Exists = false };
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<ProductStockCheckResult>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? new ProductStockCheckResult { ProductId = productId, Exists = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok kontrolünde hata. ProductId: {ProductId}", productId);
                return new ProductStockCheckResult { ProductId = productId, Exists = false };
            }
        }
    }
}