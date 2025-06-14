namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(Guid id)
            : base($"ID: {id} olan ürün bulunamadı") { }
    }
}