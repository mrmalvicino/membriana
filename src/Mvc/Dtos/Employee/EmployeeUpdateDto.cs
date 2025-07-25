using Mvc.Dtos.Person;

namespace Mvc.Dtos.Employee
{
    public class EmployeeUpdateDto : PersonUpdateDto
    {
        public DateTime AdmissionDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
