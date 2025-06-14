namespace ProductApp.Application.Products.Inputs;

public record AddProductCommandInput
{
    public string ProductName { get; init; }
    public int Quantity { get; init; }
}