using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.InvestmentRequests
{
    public class InvestmentrequestDisplay
    {
        public int InvestmentRequestId { get; set; }
        public int InvestorId { get; set; }
        public string cycleName {  get; set; }
        public int CycleId {  get; set; }
        public string farmerName {  get; set; }
        public  DateTime RequestDate { get; set; }
        public string RequestedProfitType { get; set; }
       

        public decimal RequestedAmount { get; set; }
        public string RequestStatus {  get; set; }

    }
}
