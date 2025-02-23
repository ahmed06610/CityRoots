using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleController : ControllerBase
    {
        private readonly ICycleService _cycleService;

        public CycleController(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }
        [HttpGet("GetAllOpenCyclesOfFarmer")]
        public async Task<IActionResult> GetAllOpenCycles(int FarmerId = 0)
        {
            var cycles = await _cycleService.GetAllOpenCyclesForFarmersAsync(FarmerId);
            return Ok(cycles);
        }

        [HttpGet("GetAllCycleasOfFarmerId")]
        public async Task<IActionResult> GetAllCycles(int FarmerId = 0,bool ForFarmer = true)
        {
            var cycles = await _cycleService.GetAllCyclesForFarmersAsync(FarmerId, ForFarmer);
            return Ok(cycles);
        }
        [HttpGet("BrowsingCycleasForInvestors")]
        public async Task<IActionResult> GetAllCycles()
        {
            var cycles = await _cycleService.GetAllCyclesForInvestorsAsync();
            return Ok(cycles);
        }
        [HttpGet("GetAllCycleasOfInvestor")]
        public async Task<IActionResult> GetAllCycles(int InvestorId)
        {
            var cycles = await _cycleService.GetAllPrivateCyclesForInvestor(InvestorId: InvestorId);
            return Ok(cycles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCycleById(int id)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(id);
            if (cycle == null) return NotFound();
            return Ok(cycle);
        }

        [HttpGet("GetCycleForInvestor")]
        public async Task<IActionResult> GetCycleForInvestor(int cycleId, int investorId)
        {
            var cycle = await _cycleService.GetCycleByIdForInvestorAsync(cycleId, investorId);
            if (cycle == null) return NotFound();
            return Ok(cycle);
        }

        [HttpPost("AddCycle")]
        public async Task<IActionResult> AddCycle([FromBody] CreateCycleDTO createCycleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdCycle = await _cycleService.AddCycleAsync(createCycleDto);
                return CreatedAtAction(nameof(GetCycleById), new { id = createdCycle.CycleId }, createdCycle);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut("EditCycle")]
        public async Task<IActionResult> UpdateCycle([FromBody] UpdateCycleDTO updateCycleDto)
        {

            var updatedCycle = await _cycleService.UpdateCycleAsync(updateCycleDto);
            if (updatedCycle == null) return NotFound();

            return Ok(updatedCycle);
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCycle(int id)
        {
            var success = await _cycleService.DeleteCycleAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
