using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleCommandValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Sale number is required and cannot exceed 50 characters");
        }
    }
} 