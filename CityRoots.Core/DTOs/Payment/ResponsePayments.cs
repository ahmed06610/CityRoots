using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class ResponsePayments
    {
        public List<PaymentDetailsDTO> PaymentDetails { get; set; }
        public List<PaymentsSummaryDTO> PaymentsSummary { get; set; }
    }
}
