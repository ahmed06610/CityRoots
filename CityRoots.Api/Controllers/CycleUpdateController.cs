using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.Interfaces.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleUpdateController : ControllerBase
    {
        private readonly ICycleUpdateService _cycleUpdateService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ICycleNotificationService _cycleNotificationService;

        public CycleUpdateController(ICycleUpdateService cycleUpdateService
            ,IBackgroundJobClient backgroundJobClient
            ,ICycleNotificationService cycleNotificationService)
        {
            _cycleUpdateService = cycleUpdateService;
            _backgroundJobClient = backgroundJobClient;
            _cycleNotificationService = cycleNotificationService;
        }

        [HttpGet("cycle/{cycleId}")]
        [Authorize(Roles ="Farmer,Investor")]
        public async Task<IActionResult> GetAllByCycleId(int cycleId)
        {
            var updates = await _cycleUpdateService.GetAllUpdatesByCycleIdAsync(cycleId);
            if (updates == null || updates.Count() <= 0)
                return NotFound();

            return Ok(updates);
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> Create([FromForm] CreateCycleUpdateDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var userName = User?.FindFirst("NameOfuser")?.Value;
            if (dto == null) return BadRequest("Invalid data.");
            

            var createdUpdate = await _cycleUpdateService.CreateCycleUpdateAsync(dto);
            _backgroundJobClient.Enqueue(() =>
            _cycleNotificationService.NotifyInvestorOnCyclesUpdates(dto.CycleId, userName)
            );
            return CreatedAtAction(nameof(Create), new { id = createdUpdate.UpdateId }, createdUpdate);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> Update([FromForm] UpdateCycleUpdateDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            var updatedUpdate = await _cycleUpdateService.UpdateCycleUpdateAsync(dto);
            return Ok(updatedUpdate);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cycleUpdateService.DeleteCycleUpdateAsync(id);
            if (!result) return BadRequest("Deletion failed.");

            return NoContent();
        }
    }
}
