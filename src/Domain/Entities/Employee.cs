using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Employee : Person
    {
        [Display(Name = "Fecha de admisión")]
        public DateTime AdmissionDate { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;
    }
}
