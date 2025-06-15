using ProductApp.Domain.Aggregates.Product.Exceptions;
using ProductApp.Domain.Aggregates.Product.ValueObject;
using ProductApp.Domain.Base;

namespace ProductApp.Domain.Aggregates.Product;

public sealed class Product : IAggregateRoot
{
    public Guid Id { get; private set; }// Yeni ürün oluşturulduğunda benzersiz bir ID atanır new Guid() kullanmasaydık oluşan her ürünün id si aynı olurdu(güncelleme: oluşan her ürünün id sinin aynı kalması gerektiğini söyledi gpt fakat constructora taşıdım)
    public string Name { get; private set; }
    public Money Price { get; private set; } //Primitive obsession anti-pattern'ini önler(bu pattern, primitive tiplerin aşırı kullanımını ifade eder ve bu durum kodun okunabilirliğini ve bakımını zorlaştırır)
    public int Stock { get; private set; }

    private Product() { } // EF Core için veri tabanı işlemleri için parametresiz yapıcı gereklidir veri okumada reflection için

    public Product(string name, Money price, int stock)// Parametreli yapıcı, ürün oluşturulurken gerekli bilgileri alır
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidProductNameException();

        if (stock < 0)
            throw new InvalidStockException();

        Id = Guid.NewGuid(); // Yeni ürün oluşturulduğunda benzersiz bir ID atanır(bunu buraya tasıdım ama   bunu sor tunaya
        Name = name;
        Price = price;
        Stock = stock;
    }

    public static Product Create(CreateProductModel model)// Bu metot, dışarıdan gelen modelden yeni bir Product nesnesi oluşturur
    {
        var price = new Money(model.Price);
        return new Product(model.Name, price, model.Stock);
    }
    public void AddStock(int quantity)// Bu metot, ürünün stok miktarını artırır yani ürün ekler
    {
        if (quantity <= 0)
            throw new InvalidAddProductStockException();

        Stock += quantity;
    }
    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Düşürülecek miktar pozitif olmalıdır.");

        if (Stock < quantity)
            throw new InvalidOperationException($"Yetersiz stok. Mevcut: {Stock}, İstenen: {quantity}");

        Stock -= quantity;
    }
}
