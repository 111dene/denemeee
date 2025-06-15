using ProductApp.Api.EndpointHandlers;

namespace ProductApp.Api.EndpointMappings;

public static class ProductEndpointMappings
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var productEndpoints = endpointRouteBuilder.MapGroup("/api/products");

        productEndpoints.MapPost("/CreateProduct", ProductHandlers.CreateProduct)
            .WithName("CreateProduct");

        productEndpoints.MapGet("/GetProductList", ProductHandlers.GetProductList)
            .WithName("GetProductList");

        productEndpoints.MapPost("/AddProduct", ProductHandlers.AddProduct)
            .WithName("AddProduct");

        productEndpoints.MapGet("/CheckStock/{productId:guid}", ProductHandlers.CheckStock)
            .WithName("CheckStock");
        productEndpoints.MapPost("/ProcessSale", ProductHandlers.ProcessSale)
            .WithName("ProcessSale");
    }
}