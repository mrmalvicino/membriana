using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AppUser : IdentityUser, ITenantable
    {
        [Required]
        public int OrganizationId { get; set; }
        [ValidateNever]
        public virtual Organization Organization { get; set; } = null!;
    }
}
