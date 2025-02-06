using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.InvestmentRequests
{
    public class CreateInvestmentRequest
    {
        public int InvestorId{  get; set; }    
        public int CycleId {  get; set; }
        [Required]
        public string RequestedProfitType { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]

        public decimal RequestedAmount { get; set; }

    }
}
