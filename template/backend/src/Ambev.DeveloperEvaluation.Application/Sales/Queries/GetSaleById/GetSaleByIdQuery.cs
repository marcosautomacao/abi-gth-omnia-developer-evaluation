using System;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSaleById
{
    public class GetSaleByIdQuery : IRequest<SaleDto>
    {
        public Guid Id { get; set; }
    }
} 