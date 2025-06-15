using FluentValidation;
using ProductApp.Api.Models.Requests;

namespace ProductApp.Api.Validators
{
    public class SaleProductRequestValidator : AbstractValidator<SaleProductRequest>
    {
        public SaleProductRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Sipariş ID'si boş olamaz");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("En az bir ürün seçilmelidir")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("Satış yapılacak ürün listesi boş olamaz");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("Ürün ID'si boş olamaz");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Satış miktarı 0'dan büyük olmalıdır")
                    .LessThanOrEqualTo(1000)
                    .WithMessage("Tek seferde en fazla 1000 adet satılabilir");
            });
        }
    }
}