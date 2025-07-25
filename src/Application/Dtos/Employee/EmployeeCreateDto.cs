using Application.Dtos.Person;
using Domain.Interfaces;

namespace Application.Dtos.Employee
{
    public class EmployeeCreateDto : PersonCreateDto, ITenantable
    {
        public DateTime AdmissionDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
