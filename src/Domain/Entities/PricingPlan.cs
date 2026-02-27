using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Representa un tipo de servicio o plan que una organización puede adquirir.
/// Determina el costo del servicio y las características de Membriana que la
/// organización pueda usar.
/// </summary>
/// <remarks>
/// Esta entidad modela la información persistida de un plan (nombre, monto
/// y organizaciones asociadas). Su categorización funcional se define a
/// través del enum <see cref="Domain.Enums.PricingPlan"/>.
/// </remarks>
public class PricingPlan
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
    [Display(Name = "Monto")]
    public decimal Amount { get; set; }
    #endregion

    #region Organizations
    [ValidateNever]
    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    #endregion
}
