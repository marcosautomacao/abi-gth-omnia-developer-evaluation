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
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
} 