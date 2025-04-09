using CityRoots.Api.Helpers;
using CityRoots.Core.DTOs.Farm;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService _farmService;

        public FarmController(IFarmService farmService)
        {
            _farmService = farmService;
        }

        [HttpGet("GetAllFarmasOfFarmerId")]
        [Authorize]
        public async Task<IActionResult> GetAllFarms()
        {
            var FarmerId = User.GetLoggedInId();
            if (FarmerId is null)
                return Unauthorized();
            var farms = await _farmService.GetAllFarmsAsync(FarmerId.Value);
            return Ok(farms);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFarmById(int id)
        {
            var farm = await _farmService.GetFarmByIdAsync(id);
            if (farm == null) return NotFound();
            return Ok(farm);
        }

        [HttpPost("AddFarm")]
        [Authorize("Farmer")]

        public async Task<IActionResult> AddFarm([FromBody] CreateFarmDTO createFarmDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdFarm = await _farmService.AddFarmAsync(createFarmDto);
            return CreatedAtAction(nameof(GetFarmById), new { id = createdFarm.FarmId }, createdFarm);
        }

        [HttpPut("EditFarm")]
        [Authorize("Farmer")]

        public async Task<IActionResult> UpdateFarm([FromBody] UpdateFarmDTO updateFarmDto)
        {

            var updatedFarm = await _farmService.UpdateFarmAsync(updateFarmDto);
            if (updatedFarm == null) return NotFound();

            return Ok(updatedFarm);
        }


        [HttpDelete("Delete/{id}")]
        [Authorize("Farmer")]

        public async Task<IActionResult> DeleteFarm(int id)
        {
            var success = await _farmService.DeleteFarmAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }

}
