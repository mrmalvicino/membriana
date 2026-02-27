using Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Representa un usuario de la aplicación.
/// Hereda de IdentityUser para la gestión de identidad y autenticación.
/// </summary>
public class AppUser : IdentityUser, ITenantable
{
    #region Organization
    [Required]
    public int OrganizationId { get; set; }
    [ValidateNever]
    public virtual Organization Organization { get; set; } = null!;
    #endregion

    #region Employee
    public virtual Employee? Employee { get; set; }
    #endregion

    #region Member
    public virtual Member? Member { get; set; }
    #endregion
}
