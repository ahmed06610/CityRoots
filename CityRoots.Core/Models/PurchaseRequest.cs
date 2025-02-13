using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class PurchaseRequest
    {
    
        [Key]
        public int PurchaseRequestId { get; set; }

        [Required]
        public int HarvestId { get; set; }

        [ForeignKey(nameof(HarvestId))]
        public virtual Harvest Harvest { get; set; }

        [Required]
        public int MerchantId { get; set; }

        [ForeignKey(nameof(MerchantId))]
        public virtual Merchant Merchant { get; set; }

        [Required]
        public double RequestedAmount { get; set; } // الكمية المطلوبة

        [Required]
        public decimal RequestedPrice { get; set; } // السعر المطلوب

        public string RequestStatus { get; set; } // Pending, Approved, Rejected

        public DateTime RequestDate { get;   set; } // تاريخ تقديم الطلب

        public string Notes { get; set; } // ملاحظات اختيارية
    }
}