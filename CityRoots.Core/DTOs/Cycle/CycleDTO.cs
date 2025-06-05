using CityRoots.Core.DTOs.InvestmentRequests;
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
        public string ParcelName { get; set; }
        [Required]
        public int CropId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string TimeToStart { get; set; }
        public string CropName { get; set; }
        public double ExpectedYield { get; set; }
        public bool IsOpenForInvestment { get; set; } // Indicates if this cycle is open for investment
        public OpenInvestmentCycleDTO? OpenInvestmentCycleDTO { get; set; }
       

    }
    public class OpenCycle 
    {
        public int Id { get; set; }
        public string NameCycle { get; set; }
    }
    public class CycleForBrowsing : CycleDTO
    {
        public int Rate { get; set; }
    }
    public class CycleForInvestorDTO : CycleDTO
    {
        public string Statue { get; set; }
        public decimal InvestmentOfInvestor { get; set; }
    }

    public class CycleForFarmerDTO:CycleDTO
    {
        public int? NumbersOfRequestsInvestments { get; set; }

        public List<CurrentInvestors>? currentInvestors { get; set; }
        public List<RequestsForInvestment>? requestsForInvestments { get; set; }
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
    public class CurrentInvestors
    {
        public string FullName { get; set; }
        public decimal InvestmentAmount { get; set; }
    }
    public class RequestsForInvestment
    {
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string UserImageUrl { get; set; }
        public decimal InvestmentAmount { get; set; }
        public string TypeOfProfit { get; set; }
        public int InvestmentRequestId { get; set; }
    }
}
