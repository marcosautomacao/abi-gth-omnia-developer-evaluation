using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Common.Mappings
{
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            CreateMap<Sale, SaleDto>();
            CreateMap<SaleItem, SaleItemDto>();
        }
    }
} 