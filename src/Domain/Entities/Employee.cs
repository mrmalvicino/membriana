using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Representa a un empleado de la organización.
    /// </summary>
    public class Employee : Person, IIdentifiable, ITenantable
    {
        #region AdmissionDate
        [Display(Name = "Fecha de admisión")]
        [Required]
        public DateTime AdmissionDate { get; set; }
        #endregion

        #region Organization
        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
        #endregion

        #region User
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; } = null!;
        #endregion
    }
}
