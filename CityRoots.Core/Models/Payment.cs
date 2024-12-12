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
        public string Statue { get; set; } // Aceppted, Rejected
        public string Type { get; set; } // Investment, Purchase

        public int? CycleId { get; set; }
        [ForeignKey(nameof(CycleId))]
        public virtual Cycle Cycle { get; set; }

        public int? HarvestId { get; set; }
        [ForeignKey(nameof(HarvestId))]
        public virtual Harvest Harvest { get; set; }

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
