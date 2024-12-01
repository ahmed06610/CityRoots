using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.CycleUpdate
{
    public class CycleUpdatesForHarvestDto
    {
        public DateTime UpdateDate { get; set; }
        public decimal GrowthRate { get; set; }
        public string AdditionalNotes { get; set; }
        string imageUrl { get; set; }



    }
}
