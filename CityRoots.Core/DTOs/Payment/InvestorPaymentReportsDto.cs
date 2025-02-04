using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Payment
{
    public class InvestorPaymentReportsDto
    {
        public int paymentId {  get; set; }
        public DateTime Date {  get; set; }
        public decimal amount {  get; set; }
        public string Type { get; set; }
        public string CycleName {  get; set; }
        public string PayeeName {  get; set; }
        public string PaymentMethod{  get; set; }
        public string status {  get; set; }
        public string PayPalOrderId { get; set; }

    }
}
