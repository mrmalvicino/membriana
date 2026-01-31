using System.ComponentModel.DataAnnotations;

namespace Mvc.Models
{
    public class PaymentViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El importe es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0")]
        public decimal Amount { get; set; }

        [Display(Name = "Socio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un socio")]
        public int MemberId { get; set; }

        public virtual MemberViewModel? Member { get; set; }

        [Required]
        public int OrganizationId { get; set; }
    }
}
