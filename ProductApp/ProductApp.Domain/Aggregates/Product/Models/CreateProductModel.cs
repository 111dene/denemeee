namespace ProductApp.Domain.Aggregates.Product.Models
{
    public record CreateProductModel// Bu model, dışarıdan gelen veriyi temsil eder ve Product nesnesi oluşturulurken kullanılır
    {
        public string Name { get; set; }
        public decimal Price { get; set; }//Money bir domain value object. Domain katmanının dışına çıkmamalı! o yüzden burada decimal olarak tanımlıyoruz
        public int Stock { get; set; }
    }
}
