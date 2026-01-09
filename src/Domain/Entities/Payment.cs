using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Representa un pago realizado por un socio.
    /// </summary>
    public class Payment : IIdentifiable, ITenantable
    {
        #region Id
        public int Id { get; set; }
        #endregion

        #region Active
        [Display(Name = "Estado")]
        [Required]
        public bool Active { get; set; }
        #endregion

        #region DateTime
        [Display(Name = "Fecha de pago")]
        [Required]
        public DateTime DateTime { get; set; }
        #endregion

        #region Amount
        [Display(Name = "Monto")]
        [Required]
        public decimal Amount { get; set; }
        #endregion

        #region Member
        [Required]
        public int MemberId { get; set; }
        [ValidateNever]
        public virtual Member Member { get; set; } = null!;
        #endregion

        #region Organization
        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
        #endregion
    }
}
