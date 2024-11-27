using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Farm
{
    public class CreateFarmDTO
    {
        public int FarmerId { get; set; }
        public string Location { get; set; }
        public double Size { get; set; }
    }

}
