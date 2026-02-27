using Contracts.Dtos.Person;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonCreateDto, Person>();
        CreateMap<Person, PersonReadDto>();
        CreateMap<PersonUpdateDto, Person>();
    }
}
