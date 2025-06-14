namespace ProductApp.Domain.Aggregates.Product.Exceptions
{
    public class InvalidAddProductStockException : Exception
    {
        public InvalidAddProductStockException()
            : base(" Eklenecek stok miktarı 0'dan büyük olmalı") { }
    }
}