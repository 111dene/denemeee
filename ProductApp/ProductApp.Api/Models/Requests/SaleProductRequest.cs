namespace ProductApp.Api.Models.Requests
{
    public class SaleProductRequest
    {
        public List<SaleRequestItem> Items { get; set; } = new();
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public string OrderId { get; set; }
    }

    public class SaleRequestItem 
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}