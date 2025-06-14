using ProductApp.Domain.Aggregates.Product.Exceptions;

namespace ProductApp.Domain.Aggregates.Product.ValueObject;

public sealed class Money//bu value objecttir bunların idleri yoktur sadece değerleri vardır entitylerde ise id vardır
{
    public decimal Amount { get; private set; }//dışarıdan değiştirelemesin diye private set yaptık. sadece bu sınıfın içinden değiştirilebilir

    private Money() { } // EF Core için veri tabanı işlemleri için parametresiz yapıcı gereklidir veri okumada reflection için

    public Money(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidPriceException();// Fiyat 0'dan büyük olmalıdır diyip hata fırlatıyoruz

        Amount = amount;
    }
}