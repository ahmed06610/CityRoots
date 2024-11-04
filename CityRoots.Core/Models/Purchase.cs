using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int HarvestId { get; set; }

        [ForeignKey(nameof(HarvestId))]
        public virtual Harvest Harvest { get; set; }

        [Required]
        public int MerchantId { get; set; }

        [ForeignKey(nameof(MerchantId))]
        public virtual Merchant Merchant { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Status { get; set; } // Pending, Completed, Canceled
    }

}
