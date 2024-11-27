using CityRoots.Core.DTOs.LandParcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Farm
{
    public class FarmDTO
    {
        public int FarmId { get; set; }
        public string Location { get; set; }
        public double Size { get; set; }
        public List<LandParcelDTO> LandParcels { get; set; }
    }
}
