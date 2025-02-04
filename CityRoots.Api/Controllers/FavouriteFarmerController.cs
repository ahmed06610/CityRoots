using CityRoots.Core.DTOs.FavouriteFarmers;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("Favourites/{UserId}")]
        public async Task<IActionResult> Get(string UserId) {
            try {
                return Ok(await _favouriteFarmersService.GetAllFavourites(UserId));

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFarmerToFavourite(FavouriteFarmerRequestDTO request)
        {
            try {
                await _favouriteFarmersService.AddToFavourites(request.FarmerId,request.UserId);
                return Ok("Added");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFarmerFromFavourite(FavouriteFarmerRequestDTO request)
        {
            try
            {
                await _favouriteFarmersService.RemoveFromFavourites(request.FarmerId,request.UserId);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
  
}

