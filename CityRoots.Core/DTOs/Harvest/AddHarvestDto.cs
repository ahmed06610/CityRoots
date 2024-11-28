using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class AddHarvestDto
    {     
        [Required]
        public int CropId {  get; set; }
        public int? CycleId { get; set; } 
        [Required, Range(1, 100000)]
        public double Yield { get; set; }
        [Required,Range(1,60000)]
        
        public decimal Price {  get; set; }
       
        public IFormFile Image { get; set; }
      
        


    }
}
