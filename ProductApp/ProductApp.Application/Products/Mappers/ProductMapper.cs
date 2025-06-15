using ProductApp.Application.Products.Inputs;
using ProductApp.Application.Products.Models;
using ProductApp.Application.Products.Outputs;

namespace ProductApp.Application.Products.Mappers;

public class ProductMapper 
{
    public GetProductByFilterRequestModel MapToGetProductByFilterRequestModel(GetProductsQueryInput input)//input nesnesini alır ve request modeline dönüştürür
    {
        return new GetProductByFilterRequestModel
        {
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public GetProductsQueryOutput MapToGetProductsQueryOutput(GetProductsByFiltersResponseModel responseModel)//response modeli outputa dönüştürüyor sorgulama için uygun formata getiriyor
    {
        var productOutputs = responseModel.Products.Select(x => new ProductOutput
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price.Amount,
            Stock = x.Stock

        }).ToList();

        return new GetProductsQueryOutput
        {
            Products = productOutputs,
            
        };
    }

    public SaleProductResponse MapToSaleProductResponse(SaleProductOutput output)
    {
        return new SaleProductResponse
        {
            Message = "Satış işlemi başarıyla tamamlandı",
            OrderId = output.OrderId,
            Results = output.Items.Select(item => new SaleProductResultItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                SoldQuantity = item.SoldQuantity,
                RemainingStock = item.RemainingStock
            }).ToList()
        };
    }
}

//gpt abim diyor ki , burada create product command için mapper yazmadık çünkü create işlemi için bir dönüş tipi yok. Create işlemi başarılı olduğunda sadece veri kaydediliyor ve bir çıktı üretilmiyor. Eğer create işlemi başarılı olursa, genellikle bir başarı mesajı veya durum kodu döneriz. Eğer ileride create işlemi için bir çıktı modeli eklemek istersen, o zaman mapper sınıfına bir metot ekleyebilirsin.