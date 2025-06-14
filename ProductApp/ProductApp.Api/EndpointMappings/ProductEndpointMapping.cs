using ProductApp.Api.EndpointHandlers;

namespace ProductApp.Api.EndpointMappings;

public static class ProductEndpointMapping
{
    public static void RegisterProductEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var productEndpoints = endpointRouteBuilder.MapGroup("/api/products");

        productEndpoints.MapPost("/CreateProduct", ProductHandlers.CreateProduct)
            .WithName("CreateProduct")
            .WithSwagger;

        productEndpoints.MapGet("/GetProductList", ProductHandlers.GetProductList)
            .WithName("GetProductList")
            .WithOpenApi();

        productEndpoints.MapPost("/AddProduct", ProductHandlers.AddProduct)
            .WithName("AddProduct")
            .WithOpenApi();

        productEndpoints.MapGet("/CheckStock/{productId:guid}", ProductHandlers.CheckStock)
            .WithName("CheckStock")
            .WithOpenApi();
    }
}