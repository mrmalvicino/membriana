using Contracts.Dtos.Person;

namespace Contracts.Dtos.Employee;

public class EmployeeUpdateDto : PersonUpdateDto
{
    public int OrganizationId { get; set; }
}
