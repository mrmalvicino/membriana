using Contracts.Dtos.Person;
using Contracts.Interfaces;

namespace Contracts.Dtos.Employee;

public class EmployeeCreateDto : PersonCreateDto, ITenantable
{
    public DateTime AdmissionDate { get; set; }
    public int OrganizationId { get; set; }
}
