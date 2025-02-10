using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => 
                    src.Products.Select(p => new SaleItem { ProductId = p.ProductId, Quantity = p.Quantity })));

            CreateMap<Sale, CreateSaleResult>()      
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SaleDate))                          
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => 
                    src.Items.Select(p => new CreateSaleItemResult { ProductId = p.ProductId, Quantity = p.Quantity })));;
        }
    }
}