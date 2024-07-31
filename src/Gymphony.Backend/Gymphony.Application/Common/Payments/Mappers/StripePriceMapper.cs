using AutoMapper;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Enums;
using Stripe;

namespace Gymphony.Application.Common.Payments.Mappers;

public class StripePriceMapper : Profile
{
    public StripePriceMapper()
    {
        CreateMap<Price, StripePriceDto>();
        CreateMap<StripePriceDto, PriceCreateOptions>();
        CreateMap<StripePriceRecurringDto, PriceRecurringOptions>();
        CreateMap<PriceRecurring, StripePriceRecurringDto>();
        
        CreateMap<StripeProductDetails, StripePriceDto>()
            .ForMember(dest => dest.UnitAmountDecimal, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Recurring, opt => opt.MapFrom(src => new StripePriceRecurringDto
            {
                Interval = src.DurationUnit.ToLowerString(),
                IntervalCount = src.DurationCount
            }));
    }
}