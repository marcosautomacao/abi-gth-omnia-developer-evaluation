using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly IEventPublisher _eventPublisher;

        public DeleteSaleHandler(ISaleRepository SaleRepository, IEventPublisher eventPublisher)
        {
            _SaleRepository = SaleRepository;
            _eventPublisher = eventPublisher;
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

            var result = await _SaleRepository.DeleteAsync(command.Id, cancellationToken);

            _eventPublisher.PublishAsync(new SaleDeletedEvent 
            { 
                SaleId = command.Id,
                CreatedAt = DateTime.UtcNow
            });

            return result;
        }

        Task IRequestHandler<DeleteSaleCommand>.Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}