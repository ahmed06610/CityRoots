using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.Interfaces;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public FarmerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _unitOfWork.Farmer.GetAllAsync());
        [HttpGet("GetFarmerInformation")]
        public async Task<IActionResult> GetAsync(int id)
        {
          
            var farmer = await _unitOfWork.Farmer.GetByIdAsync(id);
            if (farmer == null)
                return NotFound($"No farmer with id: {id}");
            var user = await _userManager.FindByIdAsync(farmer.ApplicationUserId);
            var info = new FarmerInfoDTO
            {
                FarmerId = farmer.FarmerId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Bio = farmer.Bio,
                
            };
            return Ok(info);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateFarmerDTO model)
        {
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _unitOfWork.Farmer.GetByIdAsync(model.FarmerId) == null)
                return BadRequest($"No farmer with id: {model.FarmerId}");
            var updated = await _userManager.FindByIdAsync(_unitOfWork.Farmer.GetByIdAsync(model.FarmerId).Result.ApplicationUserId);
            updated.PhoneNumber = model.Phone;
            updated.Name = model.FarmerName;
            await _userManager.UpdateAsync(updated);
            var farmerup = await _unitOfWork.Farmer.GetByIdAsync(model.FarmerId);
            farmerup.Bio = model.Bio;
             _unitOfWork.Farmer.Update(farmerup);
            await _unitOfWork.CompleteAsync();

            var infoUpdated = new FarmerInfoDTO { Name = updated.Name, Phone = updated.PhoneNumber, Email = updated.Email, FarmerId = model.FarmerId,Bio=model.Bio };
            return Ok(infoUpdated);
        }
    }
}
