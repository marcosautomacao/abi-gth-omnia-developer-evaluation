using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale
{
    public class CancelSaleCommand : IRequest<Unit>
    {
        public string SaleNumber { get; set; }
    }
} 