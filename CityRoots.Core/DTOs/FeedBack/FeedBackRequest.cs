using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.FeedBack
{
    public class FeedBackRequest
    {
        
        [Required,Range(1,5)]
        public int Rate {  get; set; }
        [Required,MinLength(5),MaxLength(100)]
        public string Descripition {  get; set; }
       
    }
}
