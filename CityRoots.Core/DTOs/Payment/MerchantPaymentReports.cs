using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class MerchantPaymentReportsDto
    {
        public List<MerchantPaymentDetailDto> Payments { get; set; }
        public List<PaymentMerchantSummaryDto> PaymentsSummary { get; set; }
    }

    public class MerchantPaymentDetailDto
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string PayerName { get; set; }  // Buyer name
        public string PayerEmail { get; set; } // Buyer email
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public HarvestDto AssociatedHarvest { get; set; }
    }

    public class HarvestDto
    {
        public int HarvestId { get; set; }
        public string CropName { get; set; }
    }

    public class PaymentMerchantSummaryDto
    {
        public int Year { get; set; }
        public List<decimal> PurchasesPerMonth { get; set; } = new List<decimal>();
    }
}
