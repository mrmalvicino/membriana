using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de pago")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        public int MemberId { get; set; }
        public virtual Member Member { get; set; } = null!;
    }
}
