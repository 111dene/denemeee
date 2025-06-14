using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Common;
using ProductApp.Domain.Aggregates.Product;

namespace ProductApp.Infrastructure.Persistance.EntityFrameworkCore.Products
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext context;

        public ProductRepository(ProductDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(Product product, CancellationToken cancellationToken)
        {
            await context.Products.AddAsync(product, cancellationToken);
        }
        

    }
}
//veritabanına ürün eklemek için kullanılan repository sınıfı