using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Employee : Person, IIdentifiable, ITenantable
    {
        [Display(Name = "Fecha de admisión")]
        [Required]
        public DateTime AdmissionDate { get; set; }

        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
    }
}
