using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.CycleUpdates
{
    public class CycleUpdatesDto
    {
        public DateTime UpdateDate { get; set; }
        public decimal GrowthRate { get; set; }
        public string AdditionalNotes { get; set; }
        public List<string> imageUrls { get; set; }= new List<string>();



    }
}
