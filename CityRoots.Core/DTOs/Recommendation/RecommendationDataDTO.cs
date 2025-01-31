using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Reccommendation
{
    public class RecommendationDataDTO
    {
        public List<InvestorHistoryDTO> investorHistory {  get; set; }
        public List<VisitedCyclesDTO> visitedCycles { get; set; }
        public List<FavoriteFarmersDTO> favoriteFarmers { get; set; }
        public List<CycleReco> Cycles { get; set; }
    }
    public class InvestorHistoryDTO
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
        public decimal InvestedAmount { get; set; }
        public string ReturnType { get; set; }
        public string FarmerName { get; set; }
    }
    public class VisitedCyclesDTO
    {
        public int CycleId { get; set; }
        public string CycleName { get; set; }
    }
    public class FavoriteFarmersDTO
    {
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }
    }
    public class CycleReco
    {
      public int  CycleId { get; set;}
       public string CycleName { get; set;}
       public string  FarmerName { get; set;}
       public int FarmerId { get; set; }
       public string FarmLocation {  get; set;}
       public decimal InvestmentGoal {  get; set; }
       public decimal CurrentInvestment {  get; set; }
       public DateTime StartDate {  get; set; }
       public DateTime EndDate { get; set; }   
       public string CropName {  get; set; }
       public string CropType {  get; set; }
        public int CropId { get; set; }
    }
}
