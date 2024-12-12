using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Farmer;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestDisplayDto
    {
        public int HarvestId {  get; set; }
        public string ImageUrl {  get; set; }
        public string Name {  get; set; }
        public double Yield { get; set; }
        public decimal Price {  get; set; }
        public DateTime ProductionDate { get; set; }
        public CycleDetails CycleDetails { get; set; }
        public FarmerDetails FarmerDetails { get; set; }
        


    }
}
