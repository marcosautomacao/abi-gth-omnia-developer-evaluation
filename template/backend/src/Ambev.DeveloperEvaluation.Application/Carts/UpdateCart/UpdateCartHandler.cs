using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public UpdateSaleHandler(ISaleRepository SaleRepository, IMapper mapper, IEventPublisher eventPublisher)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _SaleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            _mapper.Map(command, sale);
            await _SaleRepository.UpdateAsync(sale, cancellationToken);

            var result = _mapper.Map<UpdateSaleResult>(sale);

            _eventPublisher.PublishAsync(new SaleUpdatedEvent 
            { 
                SaleId = sale.Id,
                CreatedAt = DateTime.UtcNow
            });

            return result;
        }
    }
}