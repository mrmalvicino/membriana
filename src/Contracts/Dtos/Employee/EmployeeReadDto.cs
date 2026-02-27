using Contracts.Dtos.Person;

namespace Contracts.Dtos.Employee;

public class EmployeeReadDto : PersonReadDto
{
    public DateTime AdmissionDate { get; set; }
    public int OrganizationId { get; set; }
}
