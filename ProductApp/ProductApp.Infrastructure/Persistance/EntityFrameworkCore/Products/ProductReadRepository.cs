using Microsoft.EntityFrameworkCore;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Models;
using ProductApp.Domain.Aggregates.Product;

namespace ProductApp.Infrastructure.Persistance.EntityFrameworkCore.Products;

public sealed class ProductReadRepository : IProductReadRepository
{
    private readonly ProductDbContext context;

    public ProductReadRepository(ProductDbContext context)
    {
        this.context = context;
    }

    public async Task<GetProductsByFiltersResponseModel> GetProductsByFilters(GetProductByFilterRequestModel requestModel, CancellationToken cancellationToken)
    {
        var products = await context.Products
            .OrderBy(p => p.Id)//skip ve take sonuçları sayfalarken sıralama için gereklidir order by fakat bence isme göre sanki daha mantıklı olabilir
            .Skip((requestModel.PageNumber - 1) * requestModel.PageSize)
            .Take(requestModel.PageSize)
            .ToListAsync(cancellationToken);
        //guid id ye göre sıralama yapılıyor fakat performans açısından sorunlar yaratabilirmiş
        return new GetProductsByFiltersResponseModel
        {
            Products = products,
            PageNumber = requestModel.PageNumber,
            PageSize = requestModel.PageSize
        };
    }

    public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Products
            .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
    }
    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    public async Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}