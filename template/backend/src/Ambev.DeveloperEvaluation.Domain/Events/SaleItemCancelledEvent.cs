using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemCancelledEvent : DomainEvent
    {
        public Guid ItemId { get; }
        public Guid SaleId { get; }

        public SaleItemCancelledEvent(Guid itemId, Guid saleId)
        {
            ItemId = itemId;
            SaleId = saleId;
        }
    }
} 