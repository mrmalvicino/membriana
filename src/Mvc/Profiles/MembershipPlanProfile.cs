using AutoMapper;
using Contracts.Dtos.MembershipPlan;
using Mvc.Areas.Admin.ViewModels;

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
