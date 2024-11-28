using CityRoots.Core.DTOs.Crop;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestDisplayDto
    {
        public int HarvestId {  get; set; }
        public string Name {  get; set; }
        public double Yield { get; set; }
        public decimal Price {  get; set; }
        public DateTime Date { get; set; }
        public CycleDetails CycleDetails { get; set; }
        public FarmerDetails FarmerDetails { get; set; }
        


    }
}
