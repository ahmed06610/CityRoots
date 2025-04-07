using CityRoots.Core.DTOs.FavouriteFarmers;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteFarmerController : ControllerBase
    {
        private readonly IFavouriteFarmersService _favouriteFarmersService;
        public FavouriteFarmerController(IFavouriteFarmersService favouriteFarmersService)
        {
            _favouriteFarmersService = favouriteFarmersService;

        }
        [HttpGet("Favourites/")]
        [Authorize(Roles ="Merchant,Investor")]
        public async Task<IActionResult> Get() {
            try {
                var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(string.IsNullOrEmpty(userId))
                    return Unauthorized();
                return Ok(await _favouriteFarmersService.GetAllFavourites(userId));

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Merchant,Investor")]

        public async Task<IActionResult> AddFarmerToFavourite(FavouriteFarmerRequestDTO request)
        {
            try {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();
               
                await _favouriteFarmersService.AddToFavourites(request.FarmerId,userId);
                return Ok("Added");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Merchant,Investor")]

        public async Task<IActionResult> DeleteFarmerFromFavourite(FavouriteFarmerRequestDTO request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();
                await _favouriteFarmersService.RemoveFromFavourites(request.FarmerId,userId);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
  
}

