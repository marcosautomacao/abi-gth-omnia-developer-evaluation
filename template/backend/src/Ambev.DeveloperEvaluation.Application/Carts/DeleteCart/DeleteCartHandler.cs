using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
    {
        private readonly ISaleRepository _SaleRepository;

        public DeleteSaleHandler(ISaleRepository SaleRepository)
        {
            _SaleRepository = SaleRepository;
        }

        public async Task<bool> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Sale = await _SaleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (Sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            return await _SaleRepository.DeleteAsync(command.Id, cancellationToken);
        }

        Task IRequestHandler<DeleteSaleCommand>.Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}