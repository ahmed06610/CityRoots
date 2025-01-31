using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class PaymentsSummaryDTO
    {
        public int Year { get; set; }
        public List<decimal> PurchasesPerMonth { get; set; }
        public List<decimal> InvestmentsPerMonth { get; set; }
    }

}
