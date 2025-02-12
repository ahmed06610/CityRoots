using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.DTOs.Farmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestDetailsForMerchantDTO
    {
        public FarmerInfoDTO Farmer { get; set; }
        public HarvestDetailsDTO HarvestDetails { get; set; }
        public int? PurchaseRequestId { get; set; }
        public bool IsMerchantBuyer { get; set; } = false;
        public bool RequestReview { get; set; } = false;
        public List<CycleUpdateDTO>? cycleUpdates { get; set; }

    }
    public class HarvestDetailsDTO
    {
        public int HarvestId { get; set; }
        public string CropType { get; set; }
        public string CropName { get; set; }
        public decimal Price { get; set; }
        public double QuantityAvailable { get; set; }
        public string HarvestStatus { get; set; }
        public DateTime HarvestDate { get; set; }
    }

}
