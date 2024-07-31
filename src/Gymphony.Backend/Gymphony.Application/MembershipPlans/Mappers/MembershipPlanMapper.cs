using AutoMapper;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.MembershipPlans.Mappers;

public class MembershipPlanMapper : Profile
{
    public MembershipPlanMapper()
    {
        CreateMap<DraftMembershipPlanDto, MembershipPlan>()
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => Enum.Parse<DurationUnit>(src.DurationUnit)));
        
        CreateMap<MembershipPlan, MembershipPlanDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => src.DurationUnit.ToString()));

        CreateMap<MembershipPlan, MembershipPlanDetailsDto>()
            .ForMember(dest => dest.DurationUnit, opt => opt.MapFrom(src => src.DurationUnit.ToString()));

        CreateMap<UpdateDraftMembershipPlanCommand, DraftMembershipPlanDto>();

        CreateMap<MembershipPlan, StripeProductDetails>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ProductType.MembershipPlan));
    }
}