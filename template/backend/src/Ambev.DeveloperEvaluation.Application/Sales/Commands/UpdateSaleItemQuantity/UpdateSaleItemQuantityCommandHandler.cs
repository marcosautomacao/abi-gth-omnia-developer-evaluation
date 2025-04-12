using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSaleItemQuantity
{
    public class UpdateSaleItemQuantityCommandHandler : IRequestHandler<UpdateSaleItemQuantityCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleItemQuantityCommandHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Unit> Handle(UpdateSaleItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (sale == null)
                throw new InvalidOperationException($"Sale with number {request.SaleNumber} not found");

            var item = sale.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                throw new InvalidOperationException($"Product {request.ProductId} not found in sale {request.SaleNumber}");

            item.UpdateQuantity(request.NewQuantity);
            await _saleRepository.UpdateAsync(sale);

            return Unit.Value;
        }
    }
} 