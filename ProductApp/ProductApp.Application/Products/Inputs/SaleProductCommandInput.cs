namespace ProductApp.Application.Products.Inputs;

public record SaleProductCommandInput
{
    public List<SaleProductInput> Items { get; init; } = new();
    public string OrderId { get; init; }
}

public record SaleProductInput
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}