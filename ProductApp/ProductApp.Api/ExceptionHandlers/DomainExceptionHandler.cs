using Microsoft.AspNetCore.Diagnostics;
using ProductApp.Domain.Aggregates.Product.Exceptions;

namespace ProductApp.Api.ExceptionHandlers;

public class DomainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DomainExceptionHandler> logger;

    public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        // Domain exception'larını kontrol et
        var problemDetails = exception switch
        {
            DuplicateProductNameException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Duplicate Product Name",
                exception.Message,
                "DUPLICATE_PRODUCT_NAME"),

            InvalidProductNameException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid Product Name",
                exception.Message,
                "INVALID_PRODUCT_NAME"),

            InvalidPriceException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid Price",
                exception.Message,
                "INVALID_PRICE"),

            InvalidStockException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid Stock",
                exception.Message,
                "INVALID_STOCK"),

            InvalidAddProductStockException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid Add Product Stock",
                exception.Message,
                "INVALID_ADD_PRODUCT_STOCK"),

            ProductNotFoundException => CreateProblemDetails(
                StatusCodes.Status404NotFound,
                "Product Not Found",
                exception.Message,
                "PRODUCT_NOT_FOUND"),

            _ => null
        };

        if (problemDetails == null)
        {
            return false; // Bu exception'ı handle etmiyoruz
        }

        logger.LogError("Domain Exception Occurred Message: {Message}", exception.Message);

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProductProblemDetails CreateProblemDetails(int status, string title, string detail, string code)
    {
        return new ProductProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Code = code,
            Type = "https://tools.ietf.org/html/rfc7231"
        };
    }
}