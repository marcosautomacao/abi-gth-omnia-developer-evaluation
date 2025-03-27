using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items.Select(i => new GetSaleItemResult
                {
                    ProductId = i.Product.Id,
                    UnitPrice = i.Product.Price
                }).ToList()));
        }
    }
}