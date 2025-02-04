using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Recommendation
{
    public class InvestorRecommendationResponseDTO
    {
        public int investor_id { get; set; }
        public List<int> recommended_cycle_ids { get; set; }
    }
}
