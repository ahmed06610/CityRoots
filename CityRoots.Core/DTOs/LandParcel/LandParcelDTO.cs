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
        public int FarmId { get; set; }
        public string FarmLocation { get; set; } // From related Farm
        public double Price { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
    }


}
