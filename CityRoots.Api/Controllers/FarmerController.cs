using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFarmerService _farmerService;
        public FarmerController(IUnitOfWork unitOfWork, IFarmerService farmerService)
        {
            _unitOfWork = unitOfWork;
            _farmerService = farmerService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _unitOfWork.Farmer.GetAllAsync());
        [HttpGet("GetFarmerInformation")]
        [Authorize]
        public async Task<IActionResult> GetAsync(int id)
        {
          var info =await _farmerService.GetFarmerInfo(id);
            if (info == null)
                return NotFound();
           
            return Ok(info);
        }
        [HttpPut]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> UpdateAsync(UpdateFarmerDTO model)
        {
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _unitOfWork.Farmer.GetByIdAsync(model.FarmerId) == null)
                return BadRequest($"No farmer with id: {model.FarmerId}");
            var infoUpdated = await _farmerService.UpdateFarmer(model);
                    return Ok(infoUpdated);
        }
    }
}
