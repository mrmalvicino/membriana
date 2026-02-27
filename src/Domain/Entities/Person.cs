using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Representa una persona.
/// </summary>
public class Person : IIdentifiable
{
    #region Id
    public int Id { get; set; }
    #endregion

    #region Active
    [Display(Name = "Estado")]
    [Required]
    public bool Active { get; set; } = true;
    #endregion

    #region Name
    [Required]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;
    #endregion

    #region Email
    [Required]
    public string Email { get; set; } = null!;
    #endregion

    #region Phone
    [Display(Name = "Teléfono")]
    public string? Phone { get; set; }
    #endregion

    #region Dni
    [Display(Name = "DNI")]
    public string? Dni { get; set; }
    #endregion

    #region BirthDate
    [Display(Name = "Fecha de nacimiento")]
    public DateTime BirthDate { get; set; }
    #endregion

    #region ProfileImage
    public int? ProfileImageId { get; set; }
    [ValidateNever]
    public virtual Image? ProfileImage { get; set; }
    #endregion
}
