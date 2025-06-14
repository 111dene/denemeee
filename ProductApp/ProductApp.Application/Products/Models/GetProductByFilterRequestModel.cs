namespace ProductApp.Application.Products.Models
{
    public class GetProductByFilterRequestModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
//input modeller: kullanıcıdan gelen verileri tutar
//request modeller: API katmanında kullanılan ve uygulama katmanına iletilen verileri tutar
//output modeller: Bir işlem sonrasında dönen sonuç verilerini temsil eder
//response modeller: API katmanında kullanılan ve uygulama katmanından dönen verileri tutar