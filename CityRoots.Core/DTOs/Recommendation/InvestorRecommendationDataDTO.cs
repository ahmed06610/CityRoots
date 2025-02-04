using System;
using System.Collections.Generic;

namespace CityRoots.Core.DTOs.Reccommendation
{
    public class InvestorRecommendationDataDTO
    {
        public int Investor_id { get; set; } // Matches "investor_id" in JSON
        public InvestorDataDTO Data { get; set; }  // Encapsulates nested "data" object
    }
    public class InvestorDataDTO
    {
        public List<InvestorHistoryDTO> InvestorHistory { get; set; } // Matches "investorHistory" in JSON
        public List<VisitedCyclesDTO> VisitedCycles { get; set; } // Matches "visitedCycles" in JSON
        public List<FavoriteFarmersDTO> FavoriteFarmers { get; set; } // Matches "favoriteFarmers" in JSON
        public List<CycleReco> Cycles { get; set; } // Matches "cycles" in JSON
    }

    public class InvestorHistoryDTO
    {
        public int CycleId { get; set; } // Matches "cycleId" in JSON
        public decimal InvestmentGoal { get; set; } // Matches "investmentGoal" in JSON
        public decimal InvestedAmount { get; set; } // Matches "investedAmount" in JSON
        public int CropId { get; set; } // Matches "cropId" in JSON
        public int FarmerId { get; set; } // Matches "farmerId" in JSON

    }

    public class VisitedCyclesDTO
    {
        public int CycleId { get; set; } // Matches "cycleId" in JSON
    }

    public class FavoriteFarmersDTO
    {
        public int FarmerId { get; set; } // Matches "farmerId" in JSON
    }

    public class CycleReco
    {
        public int CycleId { get; set; } // Matches "cycleId" in JSON
        public decimal InvestmentGoal { get; set; } // Matches "investmentGoal" in JSON
        public int CropId { get; set; } // Matches "cropId" in JSON
        public decimal CurrentInvestment { get; set; } // Matches "currentInvestment" in JSON
        public int FarmerId { get; set; } // Matches "farmerId" in JSON
        public DateTime StartDate { get; set; } // Matches "startDate" in JSON
    }
}