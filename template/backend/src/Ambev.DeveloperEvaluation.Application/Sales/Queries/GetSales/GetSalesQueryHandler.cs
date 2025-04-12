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

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(s =>
                    s.SaleNumber.ToLower().Contains(searchTerm) ||
                    s.CustomerName.ToLower().Contains(searchTerm) ||
                    s.BranchName.ToLower().Contains(searchTerm));
            }

            // Apply sorting
            query = ApplySorting(query, request.SortBy, request.SortDescending);

            // Project to DTO and paginate
            var dtoQuery = query.ProjectTo<SaleDto>(_mapper.ConfigurationProvider);
            
            return await PaginatedList<SaleDto>.CreateAsync(
                dtoQuery,
                request.PageNumber,
                request.PageSize);
        }

        private static IQueryable<Domain.Entities.Sale> ApplySorting(
            IQueryable<Domain.Entities.Sale> query,
            string sortBy,
            bool sortDescending)
        {
            query = (sortBy?.ToLower()) switch
            {
                "date" => sortDescending 
                    ? query.OrderByDescending(s => s.SaleDate)
                    : query.OrderBy(s => s.SaleDate),
                "number" => sortDescending
                    ? query.OrderByDescending(s => s.SaleNumber)
                    : query.OrderBy(s => s.SaleNumber),
                "customer" => sortDescending
                    ? query.OrderByDescending(s => s.CustomerName)
                    : query.OrderBy(s => s.CustomerName),
                "amount" => sortDescending
                    ? query.OrderByDescending(s => s.TotalAmount)
                    : query.OrderBy(s => s.TotalAmount),
                _ => query.OrderByDescending(s => s.SaleDate) // Default sorting
            };

            return query;
        }
    }
} 