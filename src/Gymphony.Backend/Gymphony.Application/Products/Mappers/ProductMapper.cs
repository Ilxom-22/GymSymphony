using AutoMapper;
using Gymphony.Application.Products.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Products.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => src.DurationUnit.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}