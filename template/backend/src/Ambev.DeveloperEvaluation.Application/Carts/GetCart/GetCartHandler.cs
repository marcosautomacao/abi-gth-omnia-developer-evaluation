using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly IMapper _mapper;

        public GetSaleHandler(ISaleRepository SaleRepository, IMapper mapper)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
        }

        public async Task<GetSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new GetSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Sale = await _SaleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (Sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
            
            if (Sale.Items.Any(i => i.Quantity > 20))
                throw new ValidationException($"Cannot Sell more than 20 items");

            var result = _mapper.Map<GetSaleResult>(Sale);
            return result;
        }
    }
}