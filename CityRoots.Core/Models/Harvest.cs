using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Harvest
    {
        [Key]
        public int HarvestId { get; set; }

        [Required]
        public int CropId { get; set; }

        [ForeignKey(nameof(CropId))]
        public virtual Crop Crop { get; set; }

        [Required]
        public double Yield { get; set; }

        public DateTime Date { get; set; }

        // Navigation Properties
        public virtual List<Purchase> Purchases { get; set; }
    }

}
