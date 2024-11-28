using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Cycle
{
    public class CreateCycleDTO
    {

        [Required]
        public int ParcelId { get; set; }
        [Required]
        public int CropId { get; set; }
        [Required]
        public string CycleName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public double ExpectedYield { get; set; }
    }
}
