using Application.Dtos.Person;

namespace Application.Dtos.Employee
{
    public class EmployeeReadDto : PersonReadDto
    {
        public DateTime AdmissionDate { get; set; }
        public int OrganizationId { get; set; }
    }
}
