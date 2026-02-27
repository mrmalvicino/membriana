using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Representa el tipo de membresía de un socio. Define el monto de la cuota que paga.
    /// </summary>
    public class MembershipPlan : IIdentifiable, ITenantable
    {
        #region Id
        public int Id { get; set; }
        #endregion

        #region Name
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;
        #endregion

        #region Amount
        [Required]
        [Display(Name = "Monto de cuota")]
        public decimal Amount { get; set; }
        #endregion

        #region Members
        [ValidateNever]
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();
        #endregion

        #region Organization
        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
        #endregion
    }
}
