using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.Id).NotEmpty();
            RuleFor(product => product.Title).NotEmpty().MaximumLength(100);
            RuleFor(product => product.Price).GreaterThan(0);
            RuleFor(product => product.Description).NotEmpty().MaximumLength(500);
            RuleFor(product => product.Category).NotEmpty().MaximumLength(100);
            RuleFor(product => product.Image).NotEmpty().MaximumLength(200);
            RuleFor(product => product.Rate).InclusiveBetween(0, 5);
            RuleFor(product => product.Count).GreaterThanOrEqualTo(0);
        }
    }
}