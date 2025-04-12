using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : Entity
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        private readonly List<SaleItem> _items;

        // Private constructor for ORM
        private Sale() 
        {
            _items = new List<SaleItem>();
        }

        public Sale(
            string saleNumber,
            Guid customerId,
            string customerName,
            Guid branchId,
            string branchName)
        {
            SaleNumber = saleNumber;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            SaleDate = DateTime.UtcNow;
            IsCancelled = false;
            _items = new List<SaleItem>();

            AddDomainEvent(new SaleCreatedEvent(Id));
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            var item = new SaleItem(this, productId, productName, quantity, unitPrice);
            _items.Add(item);
            RecalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled");

            IsCancelled = true;
            AddDomainEvent(new SaleCancelledEvent(Id));
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in _items)
            {
                TotalAmount += item.TotalAmount;
            }
        }
    }
} 