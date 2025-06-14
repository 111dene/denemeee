namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class InvalidStockException : Exception
    {
        public InvalidStockException()
            : base("Stok değeri 0'dan küçük olamaz.") { }
    }
}