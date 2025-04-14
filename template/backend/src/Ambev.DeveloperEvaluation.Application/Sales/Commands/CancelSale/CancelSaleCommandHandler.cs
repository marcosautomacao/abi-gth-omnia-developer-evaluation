using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Unit> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (sale == null)
                throw new InvalidOperationException($"Sale with number {request.SaleNumber} not found");

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale);

            return Unit.Value;
        }
    }
} 