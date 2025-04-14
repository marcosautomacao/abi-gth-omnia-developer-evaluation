using System;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class SaleFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? BranchId { get; set; }
        public bool? IsCancelled { get; set; }
    }
} 