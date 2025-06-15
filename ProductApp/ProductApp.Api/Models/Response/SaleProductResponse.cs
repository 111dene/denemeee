namespace ProductApp.Api.Models.Response
{
    public class SaleProductResponse
    {
        public string Message { get; set; }
        public string OrderId { get; set; }
        public List<SaleProductResultItem> Results { get; set; } = new();
    }

    public class SaleProductResultItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int SoldQuantity { get; set; }
        public int RemainingStock { get; set; }
    }
}