using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.Dtos
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<SaleItemDto> Items { get; set; }
    }
} 