using Mvc.Dtos.Member;
using Mvc.Dtos.Person;
using Mvc.Models;

namespace Mvc.Profiles
{
    public class MemberProfile : PersonProfile
    {
        public MemberProfile()
        {
            CreateMap<MemberViewModel, MemberCreateDto>()
                .IncludeBase<PersonViewModel, PersonCreateDto>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ReverseMap();

            CreateMap<MemberReadDto, MemberViewModel>()
                .IncludeBase<PersonReadDto, PersonViewModel>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlan.Id)
                )
                .ReverseMap();

            CreateMap<MemberViewModel, MemberUpdateDto>()
                .IncludeBase<PersonViewModel, PersonUpdateDto>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ReverseMap();
        }
    }
}
