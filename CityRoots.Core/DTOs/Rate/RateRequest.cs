using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Rate
{
    public class RateRequest
    {
        public string UserId { get; set; }
        public string FarmerId { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
    }
}
