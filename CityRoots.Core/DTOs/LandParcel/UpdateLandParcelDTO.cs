using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.LandParcel
{
    public class UpdateLandParcelDTO
    {
        [Required]
        public int ParcelId { get; set; }
        [Required]
        public int FarmId { get; set; }

        public string ParcelName { get; set; }

        public IFormFile Image { get; set; } // Accept image file for updates
    }

}
