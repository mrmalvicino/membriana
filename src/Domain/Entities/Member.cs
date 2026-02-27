using Contracts.Enums;
using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Representa a un socio de la organización.
/// </summary>
public class Member : Person, IIdentifiable, ITenantable
{
    #region AdmissionDate
    [Display(Name = "Fecha de admisión")]
    [Required]
    public DateTime AdmissionDate { get; set; } = DateTime.Now;
    #endregion

    #region MembershipPlan
    [Display(Name = "Membresía")]
    [Required]
    public int MembershipPlanId { get; set; }
    [Display(Name = "Membresía")]
    [ValidateNever]
    public virtual MembershipPlan MembershipPlan { get; set; } = null!;
    #endregion

    #region Payments
    [ValidateNever]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
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

    #region MemberStatus
    [Display(Name = "Estado")]
    [Required]
    public MemberStatus MemberStatus { get; set; } = MemberStatus.Active;
    #endregion
}
