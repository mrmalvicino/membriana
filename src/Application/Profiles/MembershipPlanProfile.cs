using Contracts.Dtos.MembershipPlan;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class MembershipPlanProfile : Profile
{
    public MembershipPlanProfile()
    {
        CreateMap<MembershipPlanCreateDto, MembershipPlan>();
        CreateMap<MembershipPlan, MembershipPlanReadDto>();
        CreateMap<MembershipPlanUpdateDto, MembershipPlan>();
    }
}
