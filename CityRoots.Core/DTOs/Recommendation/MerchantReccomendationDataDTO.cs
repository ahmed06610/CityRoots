using CityRoots.Core.DTOs.Reccommendation;
using System;
using System.Collections.Generic;

namespace CityRoots.Core.DTOs.Recommendation
{
    public class MerchantRecommendationDataDTO
    {
        public int Merchant_id { get; set; }  // Matches "merchant_id" in JSON
        public MerchantDataDTO Data { get; set; }  // Encapsulates nested "data" object
    }

    public class MerchantDataDTO
    {
        public List<MerchantHistoryDTO> MerchantHistory { get; set; } // Matches "merchantHistory" in JSON
        public List<VisitedHarvestsDTO> VisitedHarvests { get; set; } // Matches "visitedHarvests" in JSON
        public List<FavoriteFarmersDTO> FavoriteFarmers { get; set; } // Matches "favoriteFarmers" in JSON
        public List<HarvestReco> Harvests { get; set; } // Matches "harvests" in JSON
    }

    public class MerchantHistoryDTO
    {
        public int HarvestId { get; set; } // Matches "harvestId" in JSON
        public decimal Price { get; set; } // Matches "price" in JSON
        public double Quantity { get; set; } // Matches "quantity" in JSON
        public int FarmerId { get; set; } // Matches "farmerId" in JSON
    }

    public class VisitedHarvestsDTO
    {
        public int HarvestId { get; set; } // Matches "harvestId" in JSON
    }

    public class HarvestReco
    {
        public int HarvestId { get; set; } // Matches "harvestId" in JSON
        public decimal Price { get; set; } // Matches "price" in JSON
        public int FarmerId { get; set; } // Matches "farmerId" in JSON
    }
}
