using ProductApp.Api.Models.Requests;
using ProductApp.Application.Products.Inputs;

namespace ProductApp.Api.Models.Mappers;

public static class ProductHandlersMapper
{
    public static CreateProductCommandInput ToCreateCommandInput(CreateProductRequest request)
    {
        return new CreateProductCommandInput
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        };
    }


    public static AddProductCommandInput ToAddCommandInput(AddProductRequest request)
    {
        return new AddProductCommandInput
        {
            ProductName = request.ProductName,
            Quantity = request.Quantity,
        };
    }

    public static GetProductsQueryInput ToGetQueryInput(GetProductsRequest request)
    {
        return new GetProductsQueryInput
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public static ProductSaleCommandInput ToProcessSaleCommandInput(ProcessSaleRequest request)
    {
        return new ProcessSaleCommandInput
        {
            OrderId = request.OrderId,
            Items = request.Items.Select(i => new SaleItemInput
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}

//API request modelleri ile uygulama katmanı modelleri (command ve query inputları) arasındaki dönüşümü yapmak için kullanılan bir mapping sınıfıdır.