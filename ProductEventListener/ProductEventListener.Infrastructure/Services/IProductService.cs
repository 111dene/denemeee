namespace ProductEventListener.Infrastructure.Services;

public interface IProductService
{
    Task<ProductStockCheckResult> CheckStockAsync(Guid productId, CancellationToken cancellationToken = default);
}

public class ProductStockCheckResult
{
    public Guid ProductId { get; set; }//id
    public bool Exists { get; set; }//ürün var mı
    public bool HasStock { get; set; }//ürün stokta mı
    public int Stock { get; set; }//ürün stok miktarı
}