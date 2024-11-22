using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Helpers
{
    public class Support
    {

        [Required, MinLength(4),MaxLength(30)]
        public string subject {  get; set; }

        [Required,MinLength(10),MaxLength(200)]
        public string Description { get; set; }
   
        
    }
}
