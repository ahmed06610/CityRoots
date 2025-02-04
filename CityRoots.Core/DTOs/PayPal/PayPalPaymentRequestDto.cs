using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.PayPal
{
    public class PayPalPaymentRequestDto
    {
        public string userId {  get; set; }
        public decimal Amount { get; set; }
        public string SellerEmail { get; set; }
        public int CycleId {  get; set; }
    }
}
