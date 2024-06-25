using AutoMapper;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Stripe;

namespace Gymphony.Application.Common.Payments.Mappers;

public class StripeProductMapper : Profile
{
    public StripeProductMapper()
    {
        CreateMap<Product, StripeProductDto>();
        CreateMap<StripeProductDto, ProductCreateOptions>();
        CreateMap<StripeProductDto, ProductUpdateOptions>();

        CreateMap<StripeProductDetails, StripeProductDto>();
    }
}