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
        public Harvest()
        {
            status = "متاح";
        }

        [Key]
        public int HarvestId { get; set; }

        [Required]
        public int CropId { get; set; }

        [ForeignKey(nameof(CropId))]
        public virtual Crop Crop { get; set; }

        [Required]
        public double Yield { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string status { get; set; }

        public DateTime ProductionDate { get;  set; }
        public bool IsAlLowedToShowUpdatesToMerchant {  get; set; }

        public string ImageUrl { get; set; }

        // Navigation Properties
        public virtual List<PurchaseRequest> Purchases { get; set; }
        public int FarmerId { get; set; }
        [ForeignKey(nameof(FarmerId))]

        public Farmer Farmer { get; set; }
        public int? CycleId {  get; set; }
        [ForeignKey(nameof(CycleId))]

        public Cycle Cycle { get; set; }
    }

}
