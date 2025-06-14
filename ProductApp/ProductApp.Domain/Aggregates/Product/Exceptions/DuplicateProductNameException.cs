namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class DuplicateProductNameException : Exception
    {
        public DuplicateProductNameException()
            : base(" Ürün ismi zaten mevcut.") { }
    }
}