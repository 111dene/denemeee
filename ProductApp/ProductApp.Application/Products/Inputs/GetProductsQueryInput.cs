namespace ProductApp.Application.Products.Inputs
{
    public record GetProductsQueryInput//buda ürünleri listelemek için gerekli olan verileri tutan bir record sınıfı
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
