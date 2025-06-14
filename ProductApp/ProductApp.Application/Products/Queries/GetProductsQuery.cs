using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Inputs;
using ProductApp.Application.Products.Mappers;
using ProductApp.Application.Products.Outputs;

namespace ProductApp.Application.Products.Queries;//ürünleri filtreleyerek getirmek için kullanılan sorgu sınıfı

public sealed class GetProductsQuery : IRequest<GetProductsQueryOutput>
{
    public GetProductsQueryInput Input { get; }

    private GetProductsQuery(GetProductsQueryInput input)
    {
        Input = input;
    }

    public static GetProductsQuery Create(GetProductsQueryInput input)
    {
        return new GetProductsQuery(input);
    }
}

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsQueryOutput>
{
    private readonly IProductReadRepository productReadRepository;
    private readonly ProductMapper productMapper;

    public GetProductsQueryHandler(IProductReadRepository productReadRepository, ProductMapper productMapper)
    {
        this.productReadRepository = productReadRepository;
        this.productMapper = productMapper;// Mapper sınıfı, sorgu çıktısını uygun formata dönüştürmek için kullanılır.
    }

    public async Task<GetProductsQueryOutput> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var requestModel = productMapper.MapToGetProductByFilterRequestModel(request.Input);
        var productsResult = await productReadRepository.GetProductsByFilters(requestModel, cancellationToken).ConfigureAwait(false);


        return productMapper.MapToGetProductsQueryOutput(productsResult);
    }
}