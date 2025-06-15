using Microsoft.AspNetCore.Diagnostics;

namespace ProductApp.Api.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> logger;

    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogCritical("Default Exception mesajı: {Message}", exception.Message);

        var problemDetails = new ProductProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Sunucu Hatası",
            Detail = "Beklenmeyen bir hata oluştu",
            Code = "INTERNAL_SERVER_ERROR",
            Type = "https://tools.ietf.org/html/rfc7231"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}