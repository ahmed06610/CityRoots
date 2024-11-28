using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Cycle
{
    public class CycleDTO
    {
        [Required]
        public int CycleId { get; set; }
        [Required]
        public string CycleName { get; set; }
        [Required]
        public int ParcelId { get; set; }
        [Required]
        public int CropId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public double ExpectedYield { get; set; }
        public bool IsOpenForInvestment { get; set; } // Indicates if this cycle is open for investment
        public OpenInvestmentCycleDTO? OpenInvestmentCycleDTO { get; set; }

    }

    public class OpenInvestmentCycleDTO
    {
        public int OpenInvestmentCycleId { get; set; }
        public decimal ExpectedFinancialGoal { get; set; }
        public decimal MinimumInvestment { get; set; }
        public decimal MaximumInvestment { get; set; }
        public int MaxInvestorsAllowed { get; set; }
        public int CurrentInvestorCount { get; set; }
        public decimal CurrentTotalInvestment { get; set; }
        public string AvailableProfitTypes { get; set; } // Cash, Crop Share, Both
    }
}
