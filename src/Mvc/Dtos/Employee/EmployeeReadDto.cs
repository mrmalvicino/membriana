using Mvc.Dtos.Person;

namespace Mvc.Dtos.Employee
{
    public class EmployeeReadDto : PersonReadDto
    {
        public DateTime AdmissionDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
