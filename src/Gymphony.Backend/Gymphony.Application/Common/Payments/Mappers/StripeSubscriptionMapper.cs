using AutoMapper;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Enums;
using Stripe;

namespace Gymphony.Application.Common.Payments.Mappers;

public class StripeSubscriptionMapper : Profile
{
    public StripeSubscriptionMapper()
    {
        CreateMap<Invoice, StripeInvoiceDto>()
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.AmountPaid))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

        CreateMap<Subscription, StripeSubscriptionDto>()
            .ForMember(dest => dest.ProductType,
                opt => opt.MapFrom(src => Enum.Parse<ProductType>(src.Items.Data[0].Plan.Product.Metadata["type"])))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Items.Data[0].Plan.ProductId))
            .ForMember(dest => dest.SubscriptionStartDate, opt => opt.MapFrom(src => src.CurrentPeriodStart))
            .ForMember(dest => dest.SubscriptionEndDate, opt => opt.MapFrom(src => src.CurrentPeriodEnd));
    }
}