using AutoMapper;
using Contracts.Dtos.Person;
using Mvc.Models;

namespace Mvc.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonViewModel, PersonCreateDto>().ReverseMap();
        CreateMap<PersonReadDto, PersonViewModel>().ReverseMap();
        CreateMap<PersonViewModel, PersonUpdateDto>().ReverseMap();
    }
}
