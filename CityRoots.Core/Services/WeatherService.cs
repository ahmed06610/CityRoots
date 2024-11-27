using CityRoots.Core.DTOs.WeatherResponse;
using CityRoots.Core.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/forecast";
        private const string ApiKey = "bfbf9ca52bf9936354dc1b4f1bdfe2bc"; // Replace with your actual OpenWeather API key

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FormattedWeatherResponseDTO> GetWeatherAsync(string city, string country)
        {
            var url = $"{BaseUrl}?q={city},{country}&appid={ApiKey}&units=metric&lang=ar";  // 'lang=ar' for Arabic
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<JObject>(jsonString);

            // Get the first forecast entry
            var firstForecast = weatherData["list"][0];

            // Extract weather details
            var temperature = firstForecast["main"]["temp"].Value<double>();
            var feelsLike = firstForecast["main"]["feels_like"].Value<double>();
            var humidity = firstForecast["main"]["humidity"].Value<int>();
            var windSpeed = firstForecast["wind"]["speed"].Value<double>();
            var description = firstForecast["weather"][0]["description"].Value<string>();
            var cloudCoverage = firstForecast["clouds"]["all"].Value<int>();
            var snowVolume = firstForecast["snow"] != null
                ? firstForecast["snow"]["3h"].Value<double>()
                : 0;

            // Dynamic agricultural recommendations
            var agriculturalRecommendations = new List<AgriculturalRecommendation>();

            // Temperature-based recommendations
            if (temperature < 0)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = " حمايه المحاصيل " + " (درجه الحراره) ",
                    Details = "درجات الحرارة منخفضة للغاية، يجب حماية المحاصيل من الصقيع."
                });
            }
            else if (temperature > 30)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "الري" + " (درجه الحراره) ",
                    Details = "الطقس حار، يوصى بجدولة الري خلال الفترات الصباحية لتقليل التبخر وتحسين كفاءة الري."
                });
            }
            else
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "العمل في الأرض" + " (درجه الحراره) ",
                    Details = "الطقس مثالي للعمل في الأرض، يمكنك متابعة الأنشطة الزراعية بشكل طبيعي."
                });
            }

            // Humidity-based recommendations
            if (humidity > 80)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "حماية المحاصيل" + " (الرطوبه) ",
                    Details = "الرطوبة العالية قد تؤدي إلى تعفن النباتات وانتشار الأمراض، يُنصح بتهوية المحاصيل."
                });
            }
            else if (humidity < 30)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "الري" + " (الرطوبه) ",
                    Details = "الرطوبة منخفضة، تأكد من ري المحاصيل بشكل كافٍ."
                });
            }
            else
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "العمل في الأرض" + " (الرطوبه) ",
                    Details = "الرطوبة مناسبة للنمو، يمكنك متابعة الأنشطة الزراعية بدون مشاكل."
                });
            }

            // Wind-based recommendations
            if (windSpeed > 20)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "حماية المحاصيل" + " (الرياح) ",
                    Details = "الرياح القوية قد تؤدي إلى تلف المحاصيل، تأكد من تأمين المحاصيل ضد الرياح."
                });
            }
            else
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "العمل في الأرض" + " (الرياح) ",
                    Details = "الرياح خفيفة، الطقس جيد للعمل في الأرض."
                });
            }

            // Snow or rain recommendations
            if (snowVolume > 0)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "حماية المحاصيل" + " (الثلوج \\ الامطار) ",
                    Details = "هناك احتمال لهطول الثلوج، تأكد من حماية المحاصيل الحساسة."
                });
            }
            else if (cloudCoverage > 80)
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "مراقبة النباتات" + " (الثلوج \\ الامطار) ",
                    Details = "الغيوم الكثيفة قد تؤثر على النمو، راقب النباتات عن كثب."
                });
            }
            else
            {
                agriculturalRecommendations.Add(new AgriculturalRecommendation
                {
                    Activity = "العمل في الأرض" + " (الثلوج \\ الامطار) ",
                    Details = "الطقس مستقر، يمكنك متابعة الأنشطة الزراعية."
                });
            }

            // Calculate plant health index (simplified)
            double healthIndex = 100;
            if (temperature < 0 || temperature > 35) healthIndex -= 20;
            if (humidity > 80 || humidity < 30) healthIndex -= 15;
            if (windSpeed > 20) healthIndex -= 10;
            if (snowVolume > 0) healthIndex -= 10;
            if (cloudCoverage > 80) healthIndex -= 5;

            healthIndex = Math.Max(0, Math.Min(100, healthIndex));

            return new FormattedWeatherResponseDTO
            {
                WeatherDetails = new WeatherDetails
                {
                    Temperature = $"{temperature} درجة مئوية",
                    Humidity = $"{humidity}%",
                    WindSpeed = $"{windSpeed} كم/ساعة",
                    Description = description
                },
                AgriculturalRecommendations = agriculturalRecommendations,
                PlantHealthStatus = new PlantHealth
                {
                    HealthIndex = $"{healthIndex}%",
                    Details = healthIndex >= 80 ? "النباتات في حالة جيدة مع معدلات نمو عالية." :
                              healthIndex >= 60 ? "النباتات في حالة متوسطة، قد تحتاج إلى بعض العناية." :
                              "النباتات تواجه تحديات، يرجى المراقبة والرعاية الفورية."
                }
            };
        }
    }
}
