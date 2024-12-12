using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class CropType
    {

        public int CropTypeId { get; set; }
        public string Name { get; set; }
        public List<Crop> crops { get; set; }= new List<Crop>();
    }
}
