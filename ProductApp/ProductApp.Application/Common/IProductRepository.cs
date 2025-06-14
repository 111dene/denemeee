using ProductApp.Domain.Aggregates.Product;

namespace ProductApp.Application.Common
{
    public interface IProductRepository
    {
        Task CreateAsync(Product product, CancellationToken cancellationToken);// Ürün oluşturma işlemi 

    }
}
