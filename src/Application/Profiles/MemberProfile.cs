using Application.Dtos.Member;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberCreateDto, Member>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ForMember(dest => dest.MembershipPlan, opt => opt.Ignore());

            CreateMap<Member, MemberReadDto>();

            CreateMap<MemberUpdateDto, Member>()
                .ForMember(
                    dest => dest.MembershipPlanId,
                    opt => opt.MapFrom(src => src.MembershipPlanId)
                )
                .ForMember(dest => dest.MembershipPlan, opt => opt.Ignore());
        }
    }
}
