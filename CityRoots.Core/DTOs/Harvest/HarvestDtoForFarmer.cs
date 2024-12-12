using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestDtoForFarmer
    {
        public int HarvestId { get; set; }
        public string ImageUrl { get; set; }
        public string Name {  get; set; }
        public double Yield { get; set; }
        public decimal price {  get; set; }
        public DateTime ProductionDate { get; set; }
        public string Status {  get; set; }
    }
}
