using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleUpdateController : ControllerBase
    {
        private readonly ICycleUpdateService _cycleUpdateService;

        public CycleUpdateController(ICycleUpdateService cycleUpdateService)
        {
            _cycleUpdateService = cycleUpdateService;
        }

        [HttpGet("cycle/{cycleId}")]
        public async Task<IActionResult> GetAllByCycleId(int cycleId)
        {
            var updates = await _cycleUpdateService.GetAllUpdatesByCycleIdAsync(cycleId);
            return Ok(updates);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCycleUpdateDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            var createdUpdate = await _cycleUpdateService.CreateCycleUpdateAsync(dto);
            return CreatedAtAction(nameof(Create), new { id = createdUpdate.UpdateId }, createdUpdate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] UpdateCycleUpdateDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            var updatedUpdate = await _cycleUpdateService.UpdateCycleUpdateAsync(dto);
            return Ok(updatedUpdate);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cycleUpdateService.DeleteCycleUpdateAsync(id);
            if (!result) return BadRequest("Deletion failed.");

            return NoContent();
        }
    }
}
