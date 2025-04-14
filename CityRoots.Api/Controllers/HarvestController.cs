using CityRoots.Api.Helpers;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvestController : ControllerBase
    {
        private readonly IHarvestService _harvestService;
        public HarvestController(IHarvestService harvestService)
        {
            _harvestService = harvestService;

        }
        [HttpPost("GetAllHarvestsForFarmer")]
        [Authorize(Roles ="Farmer")]
        public async Task<IActionResult> GetAll([FromBody] string Name = null)
        {
            var farmerId = User.GetLoggedInId();
            if(farmerId is null)
                 return Unauthorized();
            try
            {
                var harvest = await _harvestService.GetAll(Name, farmerId.Value);
                if (harvest == null || harvest.Count() <= 0)
                    return NotFound();
                return Ok(harvest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("BrowsinHarvestsForMerchant")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> GetAllHarvests()
        {
            var merchantId= User.GetLoggedInId();
            if(merchantId is null)
                return Unauthorized();
            var Harvests = await _harvestService.GetAllHarvestsForMerchantsAsync(MerchantId: merchantId.Value);
            if (Harvests == null || Harvests.Count() <= 0)
                return NotFound();
            return Ok(Harvests);
        }

        [HttpGet("GetHarvestForMerchant")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> GetHarvestForMerchant(int HarvestId)
        {
            var merchantId = User.GetLoggedInId();
            if (merchantId is null)
                return Unauthorized();
            var Harvests = await _harvestService.GetHarvestByIdForMerchantAsync(HarvestId, merchantId.Value);
            if (Harvests == null) return NotFound();
            return Ok(Harvests);
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                return Ok(await _harvestService.Get(Id));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> Add([FromForm] AddHarvestDto harvest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var farmerId = User.GetLoggedInId();
                if (farmerId is null)
                    return Unauthorized();

                return Ok(await _harvestService.Add(harvest, farmerId.Value));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> Update([FromForm] UpdateHarvestDto harvest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                return Ok(await _harvestService.Update(harvest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {

                await _harvestService.Delete(Id);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetTheRequestsofHarvest/{harvestId}")]
        [Authorize]
        public async Task<IActionResult> GetAllRequests(int harvestId)
        {
            try
            {
                return Ok(await _harvestService.GetAllPurchasesRequestForHarvest(harvestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
