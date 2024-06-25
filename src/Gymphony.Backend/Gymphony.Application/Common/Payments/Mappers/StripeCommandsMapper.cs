using AutoMapper;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Queries;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Mappers;

public class StripeCommandsMapper : Profile
{
    public StripeCommandsMapper()
    {
        CreateMap<CreateCheckoutSessionCommand, GetStripePriceIdQuery>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => Enum.Parse<ProductType>(src.ProductType)));
    }
}