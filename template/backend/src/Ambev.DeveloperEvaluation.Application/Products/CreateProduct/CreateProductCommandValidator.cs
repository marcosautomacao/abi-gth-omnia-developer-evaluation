using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Title).NotEmpty().MaximumLength(100);
            RuleFor(product => product.Price).GreaterThan(0);
            RuleFor(product => product.Description).NotEmpty().MaximumLength(500);
            RuleFor(product => product.Category).NotEmpty().MaximumLength(100);
            RuleFor(product => product.Image).NotEmpty().MaximumLength(200);
            RuleFor(product => product.Rate).InclusiveBetween(0, 5);
            RuleFor(product => product.Stock)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot  Stock  more than 20 items");
        }
    }
}