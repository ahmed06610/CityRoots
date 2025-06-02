using CityRoots.Api.Helpers;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleController : ControllerBase
    {
        private readonly ICycleService _cycleService;
        private readonly ICycleNotificationService _cycleNotificationService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public CycleController(ICycleService cycleService, IBackgroundJobClient backgroundJobClient, ICycleNotificationService cycleNotificationService)
        {
            _cycleService = cycleService;
            _backgroundJobClient = backgroundJobClient;
            _cycleNotificationService = cycleNotificationService;
        }
        [HttpGet("GetAllOpenCyclesOfFarmer")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetAllOpenCycles()
        {
            var FarmerId = User.GetLoggedInId();
            if (FarmerId is null)
                return Unauthorized();


                var cycles = await _cycleService.GetAllOpenCyclesForFarmersAsync(FarmerId.Value);
            if (cycles == null||cycles.Count<=0)
                return NotFound();
            return Ok(cycles);
        }

        [HttpGet("GetAllCycleasOfFarmerId")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> GetAllCyclesForFarmer()
        {
            var FarmerId = User.GetLoggedInId();
            if (FarmerId is null)
                return Unauthorized();
            var cycles = await _cycleService.GetAllCyclesForFarmersAsync(FarmerId.Value, true);
            if (cycles == null || cycles.Count <= 0)
                return NotFound();
            return Ok(cycles);
        }
        [HttpGet("BrowsingCycleasForInvestors")]
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetAllCyclesForInvestor()
        {
            var cycles = await _cycleService.GetAllCyclesForInvestorsAsync();
            if (cycles == null || cycles.Count <= 0)
                return NotFound();
            return Ok(cycles);
        }
        [HttpGet("GetAllCycleasOfInvestor")]
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetAllCyclesOfInvestor()
        {
            var InvestorId = User.GetLoggedInId();
            if (InvestorId is null)
                return Unauthorized();

            var cycles = await _cycleService.GetAllPrivateCyclesForInvestor(InvestorId: InvestorId.Value);
            if (cycles == null || cycles.Count <= 0)
                return NotFound();
            return Ok(cycles);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCycleById(int id)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(id);
            if (cycle == null )
                return NotFound();
            return Ok(cycle);
        }

        [HttpGet("GetCycleForInvestor")]
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetCycleForInvestor(int cycleId)
        {
            var InvestorId = User.GetLoggedInId();
            if (InvestorId is null)
                return Unauthorized();
            var cycle = await _cycleService.GetCycleByIdForInvestorAsync(cycleId, InvestorId.Value);
            if (cycle == null) return NotFound();
            return Ok(cycle);
        }

        [HttpPost("AddCycle")]
        [Authorize(Roles = "Farmer")]
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
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> UpdateCycle([FromBody] UpdateCycleDTO updateCycleDto)
        {

            var updatedCycle = await _cycleService.UpdateCycleAsync(updateCycleDto);
            if (updatedCycle == null) return NotFound();
            var userName = User?.FindFirst("NameOfuser")?.Value;
            if (userName is null) return Unauthorized();
            _backgroundJobClient.Enqueue(() =>
            _cycleNotificationService.NotifyInvestorOnUpdateOncycle(updateCycleDto.CycleId,userName));

            return Ok(updatedCycle);
        }


        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> DeleteCycle(int id)
        {
            var success = await _cycleService.DeleteCycleAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
