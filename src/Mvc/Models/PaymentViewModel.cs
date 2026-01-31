using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name = "Monto")]
        [Required]
        public decimal Amount { get; set; }

        [Display(Name = "Socio")]
        public int MemberId { get; set; }

        public virtual MemberViewModel? Member { get; set; }

        [Required]
        public int OrganizationId { get; set; }
    }
}
