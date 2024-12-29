using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Farm
    {
        [Key]
        public int FarmId { get; set; }
        [Required]
        public string FarmName { get; set; }

        [Required]
        public int FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public virtual Farmer Farmer { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public double Size { get; set; }

        // Navigation Properties
        public virtual List<LandParcel> LandParcels { get; set; }
    }

}
