using Application.Dtos.Person;

namespace Application.Dtos.Employee
{
    public class EmployeeUpdateDto : PersonUpdateDto
    {
        public DateTime AdmissionDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
