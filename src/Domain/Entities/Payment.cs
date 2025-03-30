using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de pago")]
        [Required]
        public DateTime DateTime { get; set; }

        [Display(Name = "Monto")]
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int MemberId { get; set; }
        [ValidateNever]
        public virtual Member Member { get; set; } = null!;
    }
}
