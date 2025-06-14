using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Outputs;

namespace ProductApp.Application.Products.Queries;

public sealed class ProductStockCheckQuery : IRequest<ProductStockCheckOutput>
{
    public Guid ProductId { get; }

    private ProductStockCheckQuery(Guid productId)
    {
        ProductId = productId;
    }

    public static ProductStockCheckQuery Create(Guid productId)
    {
        return new ProductStockCheckQuery(productId);
    }
}

public sealed class ProductStockCheckQueryHandler : IRequestHandler<ProductStockCheckQuery, ProductStockCheckOutput>
{
    
    private readonly IProductReadRepository productReadRepository;
    //mapper kullanmadım çünkü sadece varlık kontrolü yapıyoruz ve dönüş tipi de ProductStockCheckOutput olduğu için mapper a gerek yok dedi gpt

    public ProductStockCheckQueryHandler(IProductReadRepository productReadRepository)
    {
        
        this.productReadRepository = productReadRepository;
    }

    public async Task<ProductStockCheckOutput> Handle(ProductStockCheckQuery request, CancellationToken cancellationToken)
    {
        var product = await productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);
        
        if (product == null)
        {
            return new ProductStockCheckOutput
            {
                ProductId = request.ProductId,
                Exists = false,
                HasStock = false,
                Stock = 0
            };
        }

        return new ProductStockCheckOutput
        {
            ProductId = request.ProductId,
            Exists = true,
            HasStock = product.Stock > 0,
            Stock = product.Stock
        };
    }
}