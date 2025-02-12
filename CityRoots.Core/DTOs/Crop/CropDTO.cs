using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Crop
{
    public class CropDTO
    {
        public int CropId { get; set; }
        public string CropName { get; set; } // Ensure this matches the property name in the JSON response
    }
}
