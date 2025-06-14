namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException()
            : base($"Fiyat 0'dan büyük olmalıdır.") { }
    }
}