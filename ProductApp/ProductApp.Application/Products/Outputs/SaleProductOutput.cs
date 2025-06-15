namespace ProductApp.Application.Products.Outputs
{
    public class SaleProductOutput
    {
        public string OrderId { get; set; }
        public List<SaleProductItemOutput> Items { get; set; } = new();
    }

    public class SaleProductItemOutput
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int SoldQuantity { get; set; }
        public int RemainingStock { get; set; }
    }
}