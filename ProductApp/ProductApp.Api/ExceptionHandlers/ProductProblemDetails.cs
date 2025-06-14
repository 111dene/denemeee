using Microsoft.AspNetCore.Mvc;

namespace ProductApp.Api.ExceptionHandlers;

public class ProductProblemDetails : ProblemDetails
{
    public string Code { get; set; } = string.Empty;
}