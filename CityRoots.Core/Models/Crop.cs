using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Crop
    {
        [Key]
        public int CropId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal CurrentPrice { get; set; }

        public decimal ExpectedPriceChange { get; set; }
        public string CropType { get; set; }


        public string RiskLevel { get; set; } // Low, Medium, High

        // Navigation Properties
        public virtual List<Cycle> Cycles { get; set; }
        public virtual List<Harvest> Harvests { get; set; }
    }

}
