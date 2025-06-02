using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Farmer
{
    public class FarmerInfoDTO
    {
        public int FarmerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public int? Rate { get; set; }
        public bool? IsFarmerInFav { get; set; }
        public string ImageUrl { get; set; }

    }
}
