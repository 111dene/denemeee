using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ProductEventListener.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;
    private readonly string _productApiBaseUrl;

    public ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)//bu service product apiye bağlanır ve stok kontrolü yapar
    {
        _httpClient = httpClient;// HttpClient, Product API ile iletişim kurmak için kullanılır
        _logger = logger;// ILogger, loglama işlemleri için kullanılır
        _productApiBaseUrl = configuration["ProductApi:BaseUrl"] ?? "https://localhost:7138";// Product API'nin base URL'si, appsettings.json'dan alınır
    }

    public async Task<ProductStockCheckResult> CheckStockAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ürün ID: {ProductId} için stok kontrol ediliyor", productId);

            var response = await _httpClient.GetAsync($"{_productApiBaseUrl}/api/Products/CheckStock/{productId}", cancellationToken);// Product API'den stok kontrolü için istek gönderilir

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);// response içeriği okunur
                var result = JsonSerializer.Deserialize<ProductStockCheckResult>(content, new JsonSerializerOptions//json response içeriği ProductStockCheckResult modeline deserialize edilir
                {
                    PropertyNameCaseInsensitive = true// JSON'daki property isimleri büyük/küçük harfe duyarsız olarak deserialize edilir
                });

                _logger.LogInformation("Ürün ID: {ProductId} stok kontrolü tamamlandı. Stokta Var: {HasStock}", productId, result?.HasStock);
                return result ?? new ProductStockCheckResult { ProductId = productId, Exists = false, HasStock = false };
            }
            else
            {
                _logger.LogWarning("Ürün ID: {ProductId} için stok kontrolü başarısız. Durum Kodu: {StatusCode}", productId, response.StatusCode);
                return new ProductStockCheckResult { ProductId = productId, Exists = false, HasStock = false };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "rün ID: {ProductId} için stok kontrolünde hata oluştu", productId);
            return new ProductStockCheckResult { ProductId = productId, Exists = false, HasStock = false };
        }
    }
}