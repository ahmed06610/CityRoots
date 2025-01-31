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

        [HttpGet]
       public async Task<IActionResult> Get(int InvestorId)
        {
          var res= await reccommendation.GetRecommendationDataAsync(InvestorId);

            if (res == null)
                return NotFound();
            return Ok(res);
        }
    }
}
