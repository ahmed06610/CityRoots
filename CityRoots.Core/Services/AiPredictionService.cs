using CityRoots.Core.DTOs.AIModel;
using CityRoots.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CityRoots.Core.Services
{
    public class AiPredictionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint = "http://127.0.0.1:8000/predict";
        private readonly IUnitOfWork _unitOfWork;

        public AiPredictionService(HttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<DiseaseResponseDTO> PredictAsync(IFormFile file)
        {
            // Read file into a byte array
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // Create MultipartFormDataContent
            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            // Send POST request
            var response = await _httpClient.PostAsync(_apiEndpoint, content);

            // Ensure success
            response.EnsureSuccessStatusCode();

            // Deserialize response
            var responseBody = await response.Content.ReadAsStringAsync();
            var prediction = JsonSerializer.Deserialize<PredictionResponseDTO>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var PredictionName = prediction.Prediction.Replace('_',' ');
            var aipredictions =await _unitOfWork.AiPredict.GetAllAsync();
            string reversedSentence = string.Join(" ", PredictionName.Split(' ').Reverse());
            var aipredict = aipredictions.Where(p => StringComparer.OrdinalIgnoreCase.Equals(PredictionName, p.EnglishName)||
            StringComparer.OrdinalIgnoreCase.Equals(reversedSentence, p.EnglishName)).SingleOrDefault();
            var res = new DiseaseResponseDTO
            {
                Name = "غير معرف",
                Diagnosis = "غير معرف",
                Recommendation = "غير معرف",
                IsIll = false
            };
            if(aipredict != null)
            {
                res.Name=aipredict.ArabicName;
                if (aipredict.IsIll == true)
                {
                    res.Diagnosis = aipredict.Diagnosis;
                    res.Recommendation = aipredict.Recommendation;
                    res.IsIll = true;
                }

            }

            return res;
        }
    }
}
