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
        [HttpGet("Favourites")]
        public async Task<IActionResult> Get() {
            try {
                return Ok(await _favouriteFarmersService.GetAllFavourites());

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{FarmerId}")]
        public async Task<IActionResult> AddFarmerToFavourite(string FarmerId)
        {
            try {
                await _favouriteFarmersService.AddToFavourites(FarmerId);
                return Ok("Added");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{FarmerId}")]
        public async Task<IActionResult> DeleteFarmerFromFavourite(string FarmerId)
        {
            try
            {
                await _favouriteFarmersService.RemoveFromFavourites(FarmerId);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
  
}

