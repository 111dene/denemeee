namespace ProductApp.Api.Models.Response
{
    public sealed class AddProductResponse
    {
        public string Message { get; set; }
        public string ProductName { get; set; }
        public int NewStockAmount { get; set; }
    }
}