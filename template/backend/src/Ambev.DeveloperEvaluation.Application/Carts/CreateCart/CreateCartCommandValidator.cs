using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(Sale => Sale.UserId).NotEmpty();
            RuleForEach(Sale => Sale.Products).SetValidator(new CreateSaleItemCommandValidator());
        }
    }

    public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItem>
    {
        public CreateSaleItemCommandValidator()
        {
            RuleFor(item => item.ProductId).NotEmpty();
            RuleFor(item => item.Quantity).GreaterThan(0);
        }
    }
}