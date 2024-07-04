using AutoMapper;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Subscriptions.Commands;

namespace Gymphony.Application.Subscriptions.Mappers;

public class SubscriptionsMapper : Profile
{
    public SubscriptionsMapper()
    {
        CreateMap<StripeInvoiceDto, CreateOrRenewSubscriptionCommand>()
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => (decimal)src.PaymentAmount / 100))
            .ForMember(dest => dest.StripeSubscriptionId, opt => opt.MapFrom(src => src.SubscriptionId));

        CreateMap<StripeSubscriptionDto, CreateOrRenewSubscriptionCommand>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());

        CreateMap<CreateOrRenewSubscriptionCommand, CreateMembershipPlanSubscriptionCommand>()
            .ForMember(dest => dest.MembershipPlanId, opt => opt.MapFrom(src => src.ProductId));
        
        CreateMap<CreateOrRenewSubscriptionCommand, CreateCourseSubscriptionCommand>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.ProductId));

        CreateMap<SubscribeForMembershipPlanCommand, CreateCheckoutSessionCommand>();
    }
}