using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class LandParcel
    {
        [Key]
        public int ParcelId { get; set; }

        [Required]
        public int FarmId { get; set; }

        [ForeignKey(nameof(FarmId))]
        public virtual Farm Farm { get; set; }
        [Required]
        public string ParcelName { get; set; }

      
        public string ImageUrl {  get; set; } 

        // Navigation Properties
        public virtual List<Cycle> Cycles { get; set; }
    }

}
