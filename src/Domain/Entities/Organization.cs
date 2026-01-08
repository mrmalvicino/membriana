using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Representa una organización que utiliza Membriana.
    /// </summary>
    public class Organization : IIdentifiable
    {
        #region Id
        public int Id { get; set; }
        #endregion

        #region Active
        [Display(Name = "Estado")]
        [Required]
        public bool Active { get; set; }
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

        #region LogoImage
        public int? LogoImageId { get; set; }
        [ValidateNever]
        public virtual Image? LogoImage { get; set; }
        #endregion

        #region PricingPlan
        [Required]
        public int PricingPlanId { get; set; }
        [Required]
        public virtual PricingPlan PricingPlan { get; set; } = null!;
        #endregion

        #region Members
        [ValidateNever]
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();
        #endregion

        #region Employees
        [ValidateNever]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        #endregion

        #region MembershipPlans
        [ValidateNever]
        public virtual ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
        #endregion
    }
}
