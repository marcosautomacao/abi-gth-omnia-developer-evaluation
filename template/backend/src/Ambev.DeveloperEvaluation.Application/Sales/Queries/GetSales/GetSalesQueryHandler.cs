using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Common.Models;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, PaginatedList<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesQueryHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var query = _saleRepository.GetQueryable();

            query = ApplyFilters(query, request.Filters);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.SaleNumber.ToLower().Contains(searchTerm) ||
                    s.CustomerName.ToLower().Contains(searchTerm) ||
                    s.BranchName.ToLower().Contains(searchTerm) ||
                    s.Items.Any(i => i.ProductName.ToLower().Contains(searchTerm)));
            }

            query = ApplySorting(query, request.SortBy, request.SortDescending);

            var dtoQuery = query.ProjectTo<SaleDto>(_mapper.ConfigurationProvider);
            
            return await PaginatedList<SaleDto>.CreateAsync(
                dtoQuery,
                request.PageNumber,
                request.PageSize);
        }

        private static IQueryable<Domain.Entities.Sale> ApplyFilters(
            IQueryable<Domain.Entities.Sale> query,
            SaleFilterModel filters)
        {
            if (filters == null) return query;

            if (filters.StartDate.HasValue)
            {
                query = query.Where(s => s.SaleDate >= filters.StartDate.Value);
            }

            if (filters.EndDate.HasValue)
            {
                query = query.Where(s => s.SaleDate <= filters.EndDate.Value);
            }

            if (filters.MinAmount.HasValue)
            {
                query = query.Where(s => s.TotalAmount >= filters.MinAmount.Value);
            }

            if (filters.MaxAmount.HasValue)
            {
                query = query.Where(s => s.TotalAmount <= filters.MaxAmount.Value);
            }

            if (filters.CustomerId.HasValue)
            {
                query = query.Where(s => s.CustomerId == filters.CustomerId.Value);
            }

            if (filters.BranchId.HasValue)
            {
                query = query.Where(s => s.BranchId == filters.BranchId.Value);
            }

            if (filters.IsCancelled.HasValue)
            {
                query = query.Where(s => s.IsCancelled == filters.IsCancelled.Value);
            }

            return query;
        }

        private static IQueryable<Domain.Entities.Sale> ApplySorting(
            IQueryable<Domain.Entities.Sale> query,
            string sortBy,
            bool sortDescending)
        {
            query = (sortBy?.ToLower()) switch
            {
                GetSalesQuery.SortByOptions.Date => sortDescending 
                    ? query.OrderByDescending(s => s.SaleDate)
                    : query.OrderBy(s => s.SaleDate),
                GetSalesQuery.SortByOptions.Number => sortDescending
                    ? query.OrderByDescending(s => s.SaleNumber)
                    : query.OrderBy(s => s.SaleNumber),
                GetSalesQuery.SortByOptions.Customer => sortDescending
                    ? query.OrderByDescending(s => s.CustomerName)
                    : query.OrderBy(s => s.CustomerName),
                GetSalesQuery.SortByOptions.Branch => sortDescending
                    ? query.OrderByDescending(s => s.BranchName)
                    : query.OrderBy(s => s.BranchName),
                GetSalesQuery.SortByOptions.Amount => sortDescending
                    ? query.OrderByDescending(s => s.TotalAmount)
                    : query.OrderBy(s => s.TotalAmount),
                GetSalesQuery.SortByOptions.Status => sortDescending
                    ? query.OrderByDescending(s => s.IsCancelled)
                    : query.OrderBy(s => s.IsCancelled),
                _ => query.OrderByDescending(s => s.SaleDate) 
            };

            return query;
        }
    }
} 