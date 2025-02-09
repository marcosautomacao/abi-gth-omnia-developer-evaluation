using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(Sale => Sale.Id).NotEmpty();
            RuleFor(Sale => Sale.UserId).NotEmpty();
            RuleFor(Sale => Sale.Date).NotEmpty();
            RuleForEach(Sale => Sale.Products).SetValidator(new UpdateSaleItemCommandValidator());
        }
    }

    public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
    {
        public UpdateSaleItemCommandValidator()
        {
            RuleFor(item => item.ProductId).NotEmpty();
            RuleFor(item => item.Quantity).GreaterThan(0);
        }
    }
}