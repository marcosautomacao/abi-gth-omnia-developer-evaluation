using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSaleItemQuantity
{
    public class UpdateSaleItemQuantityCommandValidator : AbstractValidator<UpdateSaleItemQuantityCommand>
    {
        public UpdateSaleItemQuantityCommandValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Sale number is required and cannot exceed 50 characters");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required");

            RuleFor(x => x.NewQuantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(20)
                .WithMessage("Quantity must be between 1 and 20");
        }
    }
} 