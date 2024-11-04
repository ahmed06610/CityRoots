using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // e.g., PayPal, Credit Card

        [Required]
        public string PayerId { get; set; }

        [ForeignKey(nameof(PayerId))]
        public virtual ApplicationUser Payer { get; set; }

        [Required]
        public string PayeeId { get; set; }

        [ForeignKey(nameof(PayeeId))]
        public virtual ApplicationUser Payee { get; set; }
    }

}
