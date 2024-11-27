using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Farm
{
    public class UpdateFarmDTO
    {
        public int FarmId { get; set; } // Required for updates
        public string Location { get; set; }
        public double Size { get; set; }
    }

}
