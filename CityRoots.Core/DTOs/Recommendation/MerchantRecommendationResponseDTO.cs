namespace CityRoots.Core.DTOs.Recommendation
{
    public class MerchantRecommendationResponseDTO
    {
        public int merchant_id { get; set; } // Matches "merchant_id" in JSON
        public List<int> recommended_harvest_ids { get; set; } // Encapsulates the nested "data" object
    }
}
