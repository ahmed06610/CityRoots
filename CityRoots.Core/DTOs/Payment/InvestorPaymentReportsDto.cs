using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class InvestorPaymentReportsResponseDto
    {
        public List<InvestorPaymentDetailDto> Payments { get; set; }
        public List<PaymentInvestorSummaryDto> PaymentsSummary { get; set; }
    }

    public class InvestorPaymentDetailDto
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string PayeeName { get; set; }
        public string PayeeEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public CycleDto AssociatedCycle { get; set; }
    }

    public class CycleDto
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
    }

    public class PaymentInvestorSummaryDto
    {
        public int Year { get; set; }
        public List<decimal> InvestmentsPerMonth { get; set; } = new List<decimal>();
    }
}
