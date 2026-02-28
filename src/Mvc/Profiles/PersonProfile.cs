using AutoMapper;
using Contracts.Dtos.Person;
using Mvc.Areas.Admin.ViewModels;

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
