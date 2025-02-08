using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Reccommendation;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using CityRoots.Core.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class RecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICycleService _cycleService;
        private readonly IFarmerService _farmerService;
        private readonly HttpClient _httpClient;


        public RecommendationService(IUnitOfWork unitOfWork, ICycleService cycleService,
            IFarmerService farmerService, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _cycleService = cycleService;
            _farmerService = farmerService;
            _httpClient = httpClient;
        }
        public async Task<List<CycleReco>> GetAvailableCyclesAsync()
        {
            var cycles = await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(
                c => c.OpenInvestmentCycle != null,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.LandParcel.Farm.Farmer.ApplicationUser,
                c => c.Crop,
                c => c.Crop.CropType,
                c => c.OpenInvestmentCycle
            );
            return cycles.Select(c => new CycleReco
            {
                CycleId = c.CycleId,
                InvestmentGoal = c.OpenInvestmentCycle.ExpectedFinancialGoal,
                CurrentInvestment = c.OpenInvestmentCycle.CurrentTotalInvestment,
                StartDate = c.StartDate,
                CropId = c.CropId,
                FarmerId = c.LandParcel.Farm.FarmerId
            }).ToList();
        }


        public async Task<List<VisitedCyclesDTO>> GetVisitedCyclesAsync(int investorId)
        {
            var x = (await _unitOfWork.InteractionOfInvestor.FindAllWithIncludes<InteractionOfInvestor>(
                v => v.InvestorId == investorId
                )).ToList();
            var visited = new List<VisitedCyclesDTO>();
            foreach (var visit in x)
            {
                var v = new VisitedCyclesDTO
                {
                    CycleId = visit.CycleId,
                };
                visited.Add(v);
            }
            return visited;
        }
        public async Task<List<FavoriteFarmersDTO>> GetFavoriteFarmersAsync(int investorId)
        {
            var id = (await _unitOfWork.Investor.GetByIdAsync(investorId)).ApplicationUserId;
            var farmers = await _unitOfWork.FavoriteFarmers.FindAllAsync(f => f.userId == id);
            var favoritefarmers = new List<FavoriteFarmersDTO>();
            foreach (var item in farmers)
            {
                var info = await _farmerService.GetFarmerInfo((await _unitOfWork.Farmer.GetByAppUserIdAsync(item.FarmerId)).FarmerId);
                var x = new FavoriteFarmersDTO
                {
                    FarmerId = info.FarmerId,
                };
                favoritefarmers.Add(x);
            }
            return favoritefarmers;
        }
        public async Task<List<InvestorHistoryDTO>> GetInvestorHistoryAsync(int investorId)
        {
            var investments = await _unitOfWork.InvestmentRequest.FindAllWithIncludes<InvestmentRequest>(
                i => i.InvestorId == investorId,
                i => i.Cycle,
                i => i.Cycle.OpenInvestmentCycle,
                i => i.Cycle.LandParcel.Farm.Farmer,
                i => i.Cycle.LandParcel.Farm.Farmer.ApplicationUser,
                i => i.Cycle.Crop
            );

            return investments.Select(i => new InvestorHistoryDTO
            {
                CycleId = i.CycleId,
                InvestedAmount = i.RequestedAmount,
                InvestmentGoal = i.Cycle.OpenInvestmentCycle.ExpectedFinancialGoal,
                CropId = i.Cycle.CropId,
                FarmerId = i.Cycle.LandParcel.Farm.FarmerId

            }).ToList();
        }

        public async Task<List<HarvestReco>> GetAvailableHarvestsAsync()
        {
            var harvests = await _unitOfWork.Harvest.FindAllWithIncludes<Harvest>(
                 h => h.status == "Available",
                  h => h.Farmer,
                  h => h.Farmer.ApplicationUser
             );
            return harvests.Select(h => new HarvestReco
            {
                HarvestId = h.HarvestId,
                Price = h.Price,
                FarmerId = h.FarmerId,

            }).ToList();

        }
        public async Task<List<VisitedHarvestsDTO>> GetVisitedHarvestsAsync(int merchantId)
        {
            var visits = (await _unitOfWork.InteractionOfMerchant.FindAllWithIncludes<InteractionOfMerchant>(
                  v => v.MerchantId == merchantId
                 )).ToList();
            var visited = new List<VisitedHarvestsDTO>();
            foreach (var visit in visits)
            {
                var v = new VisitedHarvestsDTO
                {
                    HarvestId = visit.HarvestId,
                };
                visited.Add(v);
            }
            return visited;

        }

        public async Task<List<FavoriteFarmersDTO>> GetMerchantFavoriteFarmersAsync(int merchantId)
        {
            var id = (await _unitOfWork.Merchant.GetByIdAsync(merchantId)).ApplicationUserId;
            var farmers = await _unitOfWork.FavoriteFarmers.FindAllAsync(f => f.userId == id);
            var favoritefarmers = new List<FavoriteFarmersDTO>();
            foreach (var item in farmers)
            {
                var info = await _farmerService.GetFarmerInfo((await _unitOfWork.Farmer.GetByAppUserIdAsync(item.FarmerId)).FarmerId);
                var x = new FavoriteFarmersDTO
                {
                    FarmerId = info.FarmerId,
                };
                favoritefarmers.Add(x);
            }
            return favoritefarmers;
        }

        public async Task<List<MerchantHistoryDTO>> GetMerchantHistoryAsync(int merchantId)
        {
            var purchases = await _unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(
                 p => p.MerchantId == merchantId,
                  p => p.Harvest,
                  p => p.Harvest.Farmer
             );

            return purchases.Select(p => new MerchantHistoryDTO
            {
                HarvestId = p.HarvestId,
                Price = p.RequestedPrice,
                Quantity = p.RequestedAmount,
                FarmerId = p.Harvest.FarmerId
            }).ToList();

        }

        public async Task<List<CycleForBrowsing>> GetInvestorRecommendationDataAsync(int investorId)
        {
            var availableCycles = await GetAvailableCyclesAsync();
            var investorHistory = await GetInvestorHistoryAsync(investorId);
            var favoriteFarmers = await GetFavoriteFarmersAsync(investorId);
            var visitedCycles = await GetVisitedCyclesAsync(investorId);

            var recommendationData = new InvestorRecommendationDataDTO
            {
                Investor_id = investorId,
                Data = new InvestorDataDTO
                {
                    Cycles = availableCycles,
                    InvestorHistory = investorHistory,
                    FavoriteFarmers = favoriteFarmers,
                    VisitedCycles = visitedCycles
                }
            };

            // Serialize the RecommendationDataDTO to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(recommendationData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json");
            Console.WriteLine(jsonContent);
            // Send the data to the external API
            var response = await _httpClient.PostAsync("http://127.0.0.1:8000/recommend/investor", jsonContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Deserialize the response
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<InvestorRecommendationResponseDTO>(responseContent);


            // loop in the reccomended cyclesids and show them
            var cycles = await _cycleService.GetAllCyclesForInvestorsAsync(apiResponse);

            return cycles;
        }

        public async Task<List<Harvest>> GetMerchantRecommendationDataAsync(int merchantId)
        {
            var availableHarvests = await GetAvailableHarvestsAsync();
            var merchantHistory = await GetMerchantHistoryAsync(merchantId);
            var favoriteFarmers = await GetMerchantFavoriteFarmersAsync(merchantId);
            var visitedHarvests = await GetVisitedHarvestsAsync(merchantId);


            var recommendationData = new MerchantReccomendationDataDTO
            {
                MerchantId = merchantId,
                
                    Harvests = availableHarvests,
                    MerchantHistory = merchantHistory,
                    FavoriteFarmers = favoriteFarmers,
                    VisitedHarvests = visitedHarvests
                

            };


            // Serialize the RecommendationDataDTO to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(recommendationData), Encoding.UTF8, "application/json");

            // Send the data to the external API
            var response = await _httpClient.PostAsync("http://127.0.0.1:8000/recommend/merchant", jsonContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Deserialize the response
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<MerchantRecommendationResponseDTO>(responseContent);

            var harvests = await _unitOfWork.Harvest.FindAllAsync(h => apiResponse.RecommendedHarvestsIds.Contains(h.HarvestId));

            return harvests.ToList();

        }
        // Other methods remain unchanged...
    }
}