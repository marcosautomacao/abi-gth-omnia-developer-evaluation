using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemModifiedEvent : DomainEvent
    {
        public Guid ItemId { get; }
        public Guid SaleId { get; }

        public SaleItemModifiedEvent(Guid itemId, Guid saleId)
        {
            ItemId = itemId;
            SaleId = saleId;
        }
    }
} 