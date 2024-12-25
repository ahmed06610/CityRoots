using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.OpenInvestmentCycle
{
    public class CreateOpenInvestmentCycleDTO
    {
        public int? CycleId { get; set; }
        public decimal ExpectedFinancialGoal { get; set; }
        public decimal MinimumInvestment { get; set; }
        public decimal MaximumInvestment { get; set; }
        public int MaxInvestorsAllowed { get; set; }
        public string AvailableProfitTypes { get; set; } // Cash, Crop Share, Both
    }
}
