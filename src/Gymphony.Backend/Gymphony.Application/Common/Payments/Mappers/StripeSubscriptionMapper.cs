using AutoMapper;
using Gymphony.Application.Common.Payments.Events;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Stripe;
using Stripe.Checkout;

namespace Gymphony.Application.Common.Payments.Mappers;

public class StripeSubscriptionMapper : Profile
{
    public StripeSubscriptionMapper()
    {
        CreateMap<Invoice, StripeInvoiceDto>()
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.AmountPaid))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

        CreateMap<Subscription, StripeSubscriptionDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Items.Data[0].Plan.ProductId))
            .ForMember(dest => dest.SubscriptionStartDate, opt => opt.MapFrom(src => src.CurrentPeriodStart))
            .ForMember(dest => dest.SubscriptionEndDate, opt => opt.MapFrom(src => src.CurrentPeriodEnd));

        CreateMap<Session, StripeCheckoutSessionCompeletedEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.SubscriptionId))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.AmountTotal))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
    }
}