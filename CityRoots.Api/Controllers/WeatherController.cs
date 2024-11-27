using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}/{country}")]
    public async Task<IActionResult> GetWeather(string city, string country)
    {
        var weather = await _weatherService.GetWeatherAsync(city, country);
        if (weather == null) return NotFound("Weather data not found.");
        return Ok(weather);
    }
}