namespace ProductApp.Application.Products.Inputs;
//recurd data transfer object
public record CreateProductCommandInput//alınan verileri tutan bir record
{
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
}