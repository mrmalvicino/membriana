using Contracts.Dtos.Member;
using Contracts.Dtos.Person;
using Domain.Entities;

namespace Application.Profiles

 
{
    public class MemberProfile : PersonProfile
    {
        public MemberProfile()
        {
            CreateMap<MemberCreateDto, Member>()
                .IncludeBase<PersonCreateDto, Person>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ForMember(dest => dest.MembershipPlan, opt => opt.Ignore());

            CreateMap<Member, MemberReadDto>()
                .IncludeBase<Person, PersonReadDto>();

            CreateMap<MemberUpdateDto, Member>()
                .IncludeBase<PersonUpdateDto, Person>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ForMember(dest => dest.MembershipPlan, opt => opt.Ignore());
        }
    }
}
