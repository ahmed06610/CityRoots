using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.DTOs.Farmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Cycle
{
    public class CycleDetails
    {
        public string Name { get; set; }
        public string Location {  get; set; }
        public string LandImagesUrl { get; set; } 
        public List<CycleUpdatesForHarvestDto> CycleUpdates { get; set; } = new List<CycleUpdatesForHarvestDto>();

    }
}
