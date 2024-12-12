using CityRoots.Core.DTOs.Crop;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly ICropService _cropService;
        public CropController(ICropService cropService)
        {
            _cropService = cropService;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _cropService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                return Ok(await _cropService.Get(Id));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddCropDto crop)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                return Ok(await _cropService.Add(crop));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]  UpdateCropDto crop )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                return Ok(await _cropService.Update(crop));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {

                await _cropService.Delete(Id);  
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
