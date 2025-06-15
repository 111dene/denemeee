namespace ProductApp.Application.Products.Outputs;

public class CheckProductStockOutput
{
    public Guid ProductId { get; set; }
    public bool Exists { get; set; }
    public bool HasStock { get; set; }
    public int Stock { get; set; }
}