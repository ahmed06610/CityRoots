using CityRoots.Core.DTOs.Rate;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;
        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }
        [HttpPost]
        [Authorize(Roles = "Merchant,Investor")]

        public async Task<IActionResult> create(RateRequest rateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null) return Unauthorized();

            try
            {

                await _rateService.MakeTheRating(rateRequest,userId);
                return Ok("Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Merchant,Investor")]

        public async Task<IActionResult> Delete(DeleteRate rate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            try
            {
                await _rateService.DeleteTheRating(rate,userId);
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
