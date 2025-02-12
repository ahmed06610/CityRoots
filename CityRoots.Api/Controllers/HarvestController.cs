using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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
        public async Task<IActionResult> GetAll([FromBody] string Name = null, int farmerid = 0)
        {
            try
            {
                return Ok(await _harvestService.GetAll(Name, farmerid = 0));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("BrowsinHarvestsForMerchant")]
        public async Task<IActionResult> GetAllHarvests(int MerchantId)
        {
            var Harvests = await _harvestService.GetAllHarvestsForMerchantsAsync(MerchantId: MerchantId);
            return Ok(Harvests);
        }

        [HttpGet("GetHarvestForMerchant")]
        public async Task<IActionResult> GetHarvestForMerchant(int HarvestId, int MerchantId)
        {
            var Harvests = await _harvestService.GetHarvestByIdForMerchantAsync(HarvestId, MerchantId);
            if (Harvests == null) return NotFound();
            return Ok(Harvests);
        }

        [HttpGet("{Id}")]
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
        public async Task<IActionResult> Add([FromForm] AddHarvestDto harvest, int farmerid = 1)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                return Ok(await _harvestService.Add(harvest, farmerid));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
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
