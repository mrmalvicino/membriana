using AutoMapper;
using Contracts.Dtos.MembershipPlan;
using Mvc.Models;

namespace Mvc.Profiles;

public class MembershipPlanProfile : Profile
{
    public MembershipPlanProfile()
    {
        CreateMap<MembershipPlanCreateDto, MembershipPlanViewModel>().ReverseMap();
        CreateMap<MembershipPlanReadDto, MembershipPlanViewModel>().ReverseMap();
        CreateMap<MembershipPlanUpdateDto, MembershipPlanViewModel>().ReverseMap();

    }
}
