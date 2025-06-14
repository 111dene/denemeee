using FluentValidation;
using ProductApp.Api.Models.Requests;

namespace ProductApp.Api.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ürün ismi boş olamaz")
                .MaximumLength(100)
                .WithMessage("Ürün ismi 100 karakterden uzun olamaz");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalı");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stok 0'dan küçük olamaz");
        }
    }
}