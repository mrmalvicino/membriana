using Contracts.Dtos.Person;

namespace Contracts.Dtos.Employee;

public class EmployeeUpdateDto : PersonUpdateDto
{
    public DateTime AdmissionDate { get; set; }
    public int OrganizationId { get; set; }
}
