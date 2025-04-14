using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Common.Models;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesQuery : IRequest<PaginatedList<SaleDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
        public string SortBy { get; set; } = "date"; // Default sort by date
        public bool SortDescending { get; set; } = true; // Default newest first
        public SaleFilterModel Filters { get; set; } = new();

        public static class SortByOptions
        {
            public const string Date = "date";
            public const string Number = "number";
            public const string Customer = "customer";
            public const string Branch = "branch";
            public const string Amount = "amount";
            public const string Status = "status";
        }
    }
} 