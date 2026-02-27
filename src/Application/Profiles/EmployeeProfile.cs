using Contracts.Dtos.Employee;
using Contracts.Dtos.Person;
using Domain.Entities;

namespace Application.Profiles
{
    public class EmployeeProfile : PersonProfile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeCreateDto, Employee>()
                .IncludeBase<PersonCreateDto, Person>();

            CreateMap<Employee, EmployeeReadDto>()
                .IncludeBase<Person, PersonReadDto>();

            CreateMap<EmployeeUpdateDto, Employee>()
                .IncludeBase<PersonUpdateDto, Person>();
        }
    }
}
