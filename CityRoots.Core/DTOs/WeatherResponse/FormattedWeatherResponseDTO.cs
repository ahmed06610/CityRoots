using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.WeatherResponse
{
    public class FormattedWeatherResponseDTO
    {
        public WeatherDetails WeatherDetails { get; set; }
        public List<AgriculturalRecommendation> AgriculturalRecommendations { get; set; }
        public PlantHealth PlantHealthStatus { get; set; }
    }
}
