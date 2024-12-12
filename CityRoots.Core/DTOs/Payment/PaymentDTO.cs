using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "Investment" or "Purchase"
        public string Payer { get; set; }
        public string Payee { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // "Accepted", "Rejected"
    }
}
