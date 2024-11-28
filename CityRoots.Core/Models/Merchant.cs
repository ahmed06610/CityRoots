using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string BusinessDetails { get; set; }

        // Navigation Properties
        public virtual List<PurchaseRequest> Purchases { get; set; }
    }

}
