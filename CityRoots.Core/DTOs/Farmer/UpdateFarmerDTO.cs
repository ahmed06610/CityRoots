using CityRoots.Core.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Farmer
{
    public class UpdateFarmerDTO
    {
        [Required]
        public int FarmerId { get; set; }
        [Required, FullName]
        public string FarmerName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Bio { get; set; }

    }
}
