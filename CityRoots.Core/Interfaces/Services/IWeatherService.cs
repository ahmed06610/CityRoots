using CityRoots.Core.DTOs.WeatherResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<FormattedWeatherResponseDTO> GetWeatherAsync(string city, string country);
    }

}
