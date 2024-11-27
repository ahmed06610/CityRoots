using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandParcelController : ControllerBase
    {
        private readonly ILandParcelService _landParcelService;

        public LandParcelController(ILandParcelService landParcelService)
        {
            _landParcelService = landParcelService;
        }

        [HttpGet("GetAllLandsOfFarmId")]
        public async Task<IActionResult> GetAllLandParcels(int FarmId)
        {
            var landParcels = await _landParcelService.GetAllLandParcelsAsync(FarmId);
            return Ok(landParcels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLandParcelById(int id)
        {
            var landParcel = await _landParcelService.GetLandParcelByIdAsync(id);
            if (landParcel == null) return NotFound();
            return Ok(landParcel);
        }

        [HttpPost]
        public async Task<IActionResult> AddLandParcel([FromForm] CreateLandParcelDTO createLandParcelDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdLandParcel = await _landParcelService.AddLandParcelAsync(createLandParcelDto);
            return CreatedAtAction(nameof(GetLandParcelById), new { id = createdLandParcel.ParcelId }, createdLandParcel);
        }

        [HttpPut("EditLand")]
        public async Task<IActionResult> UpdateLandParcel([FromForm] UpdateLandParcelDTO updateLandParcelDto)
        {

            var updatedLandParcel = await _landParcelService.UpdateLandParcelAsync(updateLandParcelDto);
            if (updatedLandParcel == null) return NotFound();
            return Ok(updatedLandParcel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLandParcel(int id)
        {
            var success = await _landParcelService.DeleteLandParcelAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
