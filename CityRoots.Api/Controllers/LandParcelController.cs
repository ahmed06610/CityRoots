using CityRoots.Api.Helpers;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]

        public async Task<IActionResult> GetAllLandParcels(int FarmId)
        {
            var landParcels = await _landParcelService.GetAllLandParcelsAsync(FarmId);
            return Ok(landParcels);
        }
        [HttpGet("GetAllLandsOfFarmerId")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> GetAllLandParcelsOfFarmer()
        {
            var FarmerId = User.GetLoggedInId();

            if (FarmerId is null) return Unauthorized();
            var landParcels = await _landParcelService.GetAllLandParcelsofFarmerAsync(FarmerId.Value);
            return Ok(landParcels);
        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> GetLandParcelById(int id)
        {
            var landParcel = await _landParcelService.GetLandParcelByIdAsync(id);
            if (landParcel == null) return NotFound();
            return Ok(landParcel);
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> AddLandParcel([FromForm] CreateLandParcelDTO createLandParcelDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdLandParcel = await _landParcelService.AddLandParcelAsync(createLandParcelDto);
            return CreatedAtAction(nameof(GetLandParcelById), new { id = createdLandParcel.ParcelId }, createdLandParcel);
        }

        [HttpPut("EditLand")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> UpdateLandParcel([FromForm] UpdateLandParcelDTO updateLandParcelDto)
        {

            var updatedLandParcel = await _landParcelService.UpdateLandParcelAsync(updateLandParcelDto);
            if (updatedLandParcel == null) return NotFound();
            return Ok(updatedLandParcel);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> DeleteLandParcel(int id)
        {
            var success = await _landParcelService.DeleteLandParcelAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
