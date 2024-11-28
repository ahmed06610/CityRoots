using CityRoots.Core.DTOs.OpenInvestmentCycle;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenInvestmentCycleController : ControllerBase
    {
        private readonly IOpenInvestmentCycleService _openInvestmentCycleService;

        public OpenInvestmentCycleController(IOpenInvestmentCycleService openInvestmentCycleService)
        {
            _openInvestmentCycleService = openInvestmentCycleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOpenInvestmentCycleDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            try
            {
                var openInvestmentCycle = await _openInvestmentCycleService.CreateOpenInvestmentCycleAsync(dto);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("EditOpenCycle")]
        public async Task<IActionResult> Update([FromBody] UpdateOpenInvestmentCycleDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            try
            {
                var updatedCycle = await _openInvestmentCycleService.UpdateOpenInvestmentCycleAsync(dto);
                return Ok(updatedCycle);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _openInvestmentCycleService.DeleteOpenInvestmentCycleAsync(id);
                if (!isDeleted) return BadRequest("Deletion failed.");

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
