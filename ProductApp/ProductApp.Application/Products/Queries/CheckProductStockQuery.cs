using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Outputs;

namespace ProductApp.Application.Products.Queries;

public sealed class CheckProductStockQuery : IRequest<CheckProductStockOutput>
{
    public Guid ProductId { get; }

    private CheckProductStockQuery(Guid productId)
    {
        ProductId = productId;
    }

    public static CheckProductStockQuery Create(Guid productId)
    {
        return new CheckProductStockQuery(productId);
    }
}

public sealed class ProductStockCheckQueryHandler : IRequestHandler<CheckProductStockQuery, CheckProductStockOutput>
{
    
    private readonly IProductReadRepository productReadRepository;
    //mapper kullanmadım çünkü sadece varlık kontrolü yapıyoruz ve dönüş tipi de ProductStockCheckOutput olduğu için mapper a gerek yok dedi gpt

    public ProductStockCheckQueryHandler(IProductReadRepository productReadRepository)
    {
        
        this.productReadRepository = productReadRepository;
    }

    public async Task<CheckProductStockOutput> Handle(CheckProductStockQuery request, CancellationToken cancellationToken)
    {
        var product = await productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);
        
        if (product == null)
        {
            return new CheckProductStockOutput
            {
                ProductId = request.ProductId,
                Exists = false,
                HasStock = false,
                Stock = 0
            };
        }

        return new CheckProductStockOutput
        {
            ProductId = request.ProductId,
            Exists = true,
            HasStock = product.Stock > 0,
            Stock = product.Stock
        };
    }
}