using CityRoots.Api.Helpers;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
       public async Task<IActionResult> GetReccommendationForInvestor()
        {
            var investorId = User.GetLoggedInId();
            if(investorId is null)
                return Unauthorized();

            
          var res= await reccommendation.GetInvestorRecommendationDataAsync(investorId.Value);

            if (res == null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("ReccommendationForMerchant")]
        public async Task<IActionResult> GetReccommendationForMerchant()
        {
            var merchantId = User.GetLoggedInId();
            if (merchantId is null)
                return Unauthorized();
            var res = await reccommendation.GetMerchantRecommendationDataAsync(merchantId.Value);

            if (res == null)
                return NotFound();
            return Ok(res);
        }
    }
}
