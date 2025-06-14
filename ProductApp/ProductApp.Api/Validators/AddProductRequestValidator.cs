using FluentValidation;
using ProductApp.Api.Models.Requests;

namespace ProductApp.Api.Validators
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Ürün ismi boş olamaz")
                .MaximumLength(100)
                .WithMessage("Ürün ismi 100 karakterden uzun olamaz");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Eklenecek stok miktarı 0'dan büyük olmalı")
                .LessThanOrEqualTo(10000)
                .WithMessage("Eklenecek stok miktarı 10000'den büyük olamaz");
        }
    }
}