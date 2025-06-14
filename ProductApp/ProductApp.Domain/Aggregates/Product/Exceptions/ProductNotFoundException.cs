namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
            : base("'ürün bulunamadı") { }
    }
}