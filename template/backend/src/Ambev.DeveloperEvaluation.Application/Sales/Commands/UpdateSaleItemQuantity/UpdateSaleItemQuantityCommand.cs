using System;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSaleItemQuantity
{
    public class UpdateSaleItemQuantityCommand : IRequest<Unit>
    {
        public string SaleNumber { get; set; }
        public Guid ProductId { get; set; }
        public int NewQuantity { get; set; }
    }
} 