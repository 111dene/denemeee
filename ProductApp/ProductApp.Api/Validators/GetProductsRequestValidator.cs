using FluentValidation;
using ProductApp.Api.Models.Requests;

namespace ProductApp.Api.Validators
{
    public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Sayfa numarası 1'den büyük olmalı");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Sayfa boyutu 1-100 arasında olmalı");
        }
    }
}