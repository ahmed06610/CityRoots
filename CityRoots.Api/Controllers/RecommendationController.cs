using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService reccommendation;

        public RecommendationController(RecommendationService reccommendation)
        {
            this.reccommendation = reccommendation;
        }

        [HttpGet("ReccommendationForInvestor")]
       public async Task<IActionResult> GetReccommendationForInvestor(int InvestorId)
        {
          var res= await reccommendation.GetInvestorRecommendationDataAsync(InvestorId);

            if (res == null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("ReccommendationForMerchant")]
        public async Task<IActionResult> GetReccommendationForMerchant(int MerchantId)
        {
            var res = await reccommendation.GetMerchantRecommendationDataAsync(MerchantId);

            if (res == null)
                return NotFound();
            return Ok(res);
        }
    }
}
