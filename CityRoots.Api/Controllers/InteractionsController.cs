using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly InteractionsService interactionsService;

        public InteractionsController(InteractionsService interactionsService)
        {
            this.interactionsService = interactionsService;
        }
        [HttpPost("LogCycleForInvestor")]
        public async Task<IActionResult> LogCycle(int investorId,int cycleId)
        {
            var res=await interactionsService.VisitCycle(investorId, cycleId);
            if (res is null)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("LogHarvestForMerchant")]
        public async Task<IActionResult> LogHarvest(int merchantId, int HarvestId)
        {
            var res = await interactionsService.VisitHarvest(merchantId, HarvestId);
            if (res is null)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
