using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.WeatherResponse
{
    public class WeatherResponseDTO
{
    public string City { get; set; }
    public string Country { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Description { get; set; }
    public double WindSpeed { get; set; }
}

}
