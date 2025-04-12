using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales
{
    public class GetSalesQuery : IRequest<IEnumerable<SaleDto>>
    {
    }
} 