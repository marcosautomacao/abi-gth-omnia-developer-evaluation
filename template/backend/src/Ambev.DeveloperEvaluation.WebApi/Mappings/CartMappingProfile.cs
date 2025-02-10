using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Mappings
{
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(s => new CreateSaleItem { ProductId = s.ProductId, Quantity = s.Quantity }).ToList()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
            
            CreateMap<CreateSaleResult, CreateSaleResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(s => new CreateSaleItemResponse { ProductId = s.ProductId, Quantity = s.Quantity }).ToList()))
                ;

            CreateMap<GetSaleRequest, GetSaleCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
                
            CreateMap<GetSaleResult, GetSaleResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(s => new GetSaleItemResponse { ProductId = s.ProductId, Quantity = s.Quantity }).ToList()));

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
            CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();
            CreateMap<UpdateSaleCommand, UpdateSaleResponse>();
            CreateMap<UpdateSaleItemCommand, UpdateSaleItemResponse>();
        }
    }
}