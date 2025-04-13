using System;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : Entity
    {
        public Guid SaleId { get; private set; }
        public Sale Sale { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }

        // Private constructor for ORM
        private SaleItem() { }

        public SaleItem(
            Sale sale,
            Guid productId,
            string productName,
            int quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
            
            if (quantity > 20)
                throw new ArgumentException("Maximum quantity per item is 20", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));

            Sale = sale ?? throw new ArgumentNullException(nameof(sale));
            SaleId = sale.Id;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            IsCancelled = false;

            CalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Item is already cancelled");

            IsCancelled = true;
            AddDomainEvent(new SaleItemCancelledEvent(Id, SaleId));
        }

        private void CalculateTotalAmount()
        {
            decimal subtotal = Quantity * UnitPrice;
            
            // Apply discount rules
            if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = subtotal * 0.20m; // 20% discount
            }
            else if (Quantity >= 4)
            {
                Discount = subtotal * 0.10m; // 10% discount
            }
            else
            {
                Discount = 0;
            }

            TotalAmount = subtotal - Discount;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));
            
            if (newQuantity > 20)
                throw new ArgumentException("Maximum quantity per item is 20", nameof(newQuantity));

            if (newQuantity != Quantity)
            {
                Quantity = newQuantity;
                CalculateTotalAmount();
                AddDomainEvent(new SaleItemModifiedEvent(Id, SaleId));
            }
        }

        public void UpdateUnitPrice(decimal newUnitPrice)
        {
            if (newUnitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero", nameof(newUnitPrice));

            if (newUnitPrice != UnitPrice)
            {
                UnitPrice = newUnitPrice;
                CalculateTotalAmount();
                AddDomainEvent(new SaleItemModifiedEvent(Id, SaleId));
            }
        }
    }
} 