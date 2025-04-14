using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.LandParcel
{
    public class LandParcelDTO
    {
        public int ParcelId { get; set; }
        public string ParcelName { get; set; }
        public int FarmId { get; set; }
        public string FarmLocation { get; set; } // From related Farm
        public string Status { get; set; }
        public string? CycleName { get; set; } // From related Cycle
        public string ImageUrl { get; set; }
    }


}
