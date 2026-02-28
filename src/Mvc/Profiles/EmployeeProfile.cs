using Contracts.Dtos.Employee;
using Contracts.Dtos.Person;
using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Profiles;

public class EmployeeProfile : PersonProfile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeViewModel, EmployeeCreateDto>()
            .IncludeBase<PersonViewModel, PersonCreateDto>();

        CreateMap<EmployeeReadDto, EmployeeViewModel>()
            .IncludeBase<PersonReadDto, PersonViewModel>();

        CreateMap<EmployeeViewModel, EmployeeUpdateDto>()
            .IncludeBase<PersonViewModel, PersonUpdateDto>();
    }
}
