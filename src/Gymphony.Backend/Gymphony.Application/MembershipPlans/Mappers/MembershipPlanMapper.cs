using AutoMapper;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.MembershipPlans.Mappers;

public class MembershipPlanMapper : Profile
{
    public MembershipPlanMapper()
    {
        CreateMap<DraftMembershipPlanDto, MembershipPlan>();
        
        CreateMap<MembershipPlan, MembershipPlanDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<MembershipPlan, MembershipPlanDetailsDto>();

        CreateMap<UpdateDraftMembershipPlanCommand, DraftMembershipPlanDto>();
    }
}