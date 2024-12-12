using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll([FromBody]string Name=null)
        {
            try
            {
                return Ok(await _harvestService.GetAll(Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


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
        public async Task<IActionResult> Add([FromForm] AddHarvestDto harvest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                return Ok(await _harvestService.Add(harvest));

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
    }
}
