using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Api.EndpointMappings;
using ProductApp.Api.Models.Mappers;
using ProductApp.Api.Models.Requests;
using ProductApp.Api.Models.Response;
using ProductApp.Application.Products.Commands;
using ProductApp.Application.Products.Outputs;
using ProductApp.Application.Products.Queries;

namespace ProductApp.Api.EndpointHandlers;

public static class ProductHandlers
{
    public static async Task<Ok<CreateProductResponse>> CreateProduct(
        [FromBody] CreateProductRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var commandInput = ProductHandlersMapper.ToCreateCommandInput(request);
        var command = CreateProductCommand.Create(commandInput);

        await mediator.Send(command, cancellationToken);

        return TypedResults.Ok(new CreateProductResponse { Message = "Ürün başarıyla oluşturuldu" });
    }

    public static async Task<Ok<GetProductsQueryOutput>> GetProductList(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetProductsRequest { PageNumber = pageNumber, PageSize = pageSize };
        var queryInput = ProductHandlersMapper.ToGetQueryInput(request);
        var query = GetProductsQuery.Create(queryInput);

        var result = await mediator.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AddProductResponse>> AddProduct(
        [FromBody] AddProductRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var commandInput = ProductHandlersMapper.ToAddCommandInput(request);
        var command = AddProductCommand.Create(commandInput);

        var newStockAmount = await mediator.Send(command, cancellationToken);

        return TypedResults.Ok(new AddProductResponse
        {
            Message = "Stok başarıyla eklendi",
            ProductName = request.ProductName,
            NewStockAmount = newStockAmount
        });
    }

    public static async Task<Ok<ProductStockCheckOutput>> CheckStock(
        [FromRoute] Guid productId,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = ProductStockCheckQuery.Create(productId);
        var result = await mediator.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}