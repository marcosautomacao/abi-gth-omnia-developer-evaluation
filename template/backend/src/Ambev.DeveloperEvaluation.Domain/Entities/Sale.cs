using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale record in the system.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the total sale amount.
        /// </summary>
        public decimal TotalSaleAmount { get; set; }

        /// <summary>
        /// Gets or sets the branch where the sale was made.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the sale is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; } = false;

        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the collection of sale items.
        /// </summary>
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }

    public class SaleItem
    {
        public Product Product { get; set; }
        public Sale Sale { get; set; }
        public Guid ProductId { get; set; }
        public Guid SaleId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
    }
}