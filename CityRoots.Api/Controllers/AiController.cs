using CityRoots.Core.DTOs.AIModel;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly AiPredictionService _predictionService;

        public AiController(AiPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromForm] ImageUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Please upload a valid image file.");
            }

            try
            {
                var prediction = await _predictionService.PredictAsync(dto.File);
                return Ok(prediction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
