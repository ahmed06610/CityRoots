using CityRoots.Api.Helpers;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> LogCycle(int cycleId)
        {
            var investorId=User.GetLoggedInId();
            if(investorId is null)
                return Unauthorized();
            var res=await interactionsService.VisitCycle(investorId.Value, cycleId);
            if (res is null)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("LogHarvestForMerchant")]

        public async Task<IActionResult> LogHarvest(int HarvestId)
        {
            var merchantId = User.GetLoggedInId();
            if (merchantId is null)
                return Unauthorized();
            var res = await interactionsService.VisitHarvest(merchantId.Value, HarvestId);
            if (res is null)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
