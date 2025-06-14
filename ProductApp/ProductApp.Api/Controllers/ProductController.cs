using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Api.EndpointMappings;
using ProductApp.Api.Models.Requests;
using ProductApp.Api.Models.Response;
using ProductApp.Application.Products.Commands;
using ProductApp.Application.Products.Outputs;//bu kullanım doğrumu emin değilim 
using ProductApp.Application.Products.Queries;
using ProductApp.Domain.Aggregates.Product.Exceptions;//bu dopru bir kullanım mı emin değilim, çünkü bu hata uygulama katmanında yakalanıyor ve controller'da işleniyor.

namespace ProductApp.Api.Controllers;
//fluent validation kullanılıyor mu şuan emin değilim
[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("CreateProduct")]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        try//galiba doğru bir kullanım değil middlewareda yapmak mantıklı burayı temizlemek lazım TODO: burayı temizle
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commandInput = ProductEndpointMappings.ToCreateCommandInput(request);
            var command = CreateProductCommand.Create(commandInput);

            await mediator.Send(command, cancellationToken);

            return Ok(new CreateProductResponse { Message = "Ürün başarıyla oluşturuldu" });
        }
        catch (DuplicateProductNameException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Bir hata oluştu", Details = ex.Message });
        }
    }

    [HttpGet("GetProductList")]
    [ProducesResponseType(typeof(GetProductsQueryOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductList([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var queryInput = ProductEndpointMappings.ToGetQueryInput(request);
            var query = GetProductsQuery.Create(queryInput);

            var result = await mediator.Send(query, cancellationToken);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Bir hata oluştu", Details = ex.Message });
        }
    }

    [HttpPost("AddProduct")]
    [ProducesResponseType(typeof(AddProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commandInput = ProductEndpointMappings.ToAddCommandInput(request);
            var command = AddProductCommand.Create(commandInput);

            var newStockAmount = await mediator.Send(command, cancellationToken);

            return Ok(new AddProductResponse
            {
                Message = "Stok başarıyla eklendi",
                ProductName = request.ProductName,
                NewStockAmount = newStockAmount
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Bir hata oluştu", Details = ex.Message });
        }
    }


    [HttpGet("CheckStock/{productId}")]
    [ProducesResponseType(typeof(ProductStockCheckOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckStock(Guid productId, CancellationToken cancellationToken)
    {
        try
        {
            var query = ProductStockCheckQuery.Create(productId);//productId parametresini alır ve sorgu nesnesi oluşturur
            var result = await mediator.Send(query, cancellationToken);//mediator aracılığıyla sorguyu işler

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Bir hata oluştu", Details = ex.Message });
        }
    }

}
