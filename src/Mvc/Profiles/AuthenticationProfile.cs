using AutoMapper;
using Mvc.Dtos.Authentication;
using Mvc.Models;

namespace Mvc.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginViewModel, LoginRequestDto>();
            CreateMap<RegisterViewModel, RegisterRequestDto>();
        }
    }
}
