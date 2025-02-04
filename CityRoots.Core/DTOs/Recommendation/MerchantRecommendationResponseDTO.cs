namespace CityRoots.Core.DTOs.Recommendation
{
    public class MerchantRecommendationResponseDTO
    {
        public int MerchantId { get; set; } // Matches "merchant_id" in JSON
        public List<int> RecommendedHarvestsIds { get; set; } // Encapsulates the nested "data" object
    }
}
