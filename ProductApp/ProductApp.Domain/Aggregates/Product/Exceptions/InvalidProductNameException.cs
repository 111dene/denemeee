namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class InvalidProductNameException : Exception
    {
        public InvalidProductNameException()
            : base("Ürün ismi boş olamaz") { }

    }
}