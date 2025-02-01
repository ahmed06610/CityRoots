using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Reccommendation;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using CityRoots.Core.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class RecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICycleService _cycleService;
        private readonly IFarmerService _farmerService;

        public RecommendationService(IUnitOfWork unitOfWork, ICycleService cycleService, IFarmerService farmerService)
        {
            _unitOfWork = unitOfWork;
            _cycleService = cycleService;
            _farmerService = farmerService;
        }
        public async Task<List<CycleReco>> GetAvailableCyclesAsync()
        {
            var cycles =await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(
                c =>c.OpenInvestmentCycle!= null,
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
                CycleName = c.CycleName,
                FarmerName = c.LandParcel.Farm.Farmer.ApplicationUser.UserName,
                FarmerId = c.LandParcel.Farm.Farmer.FarmerId,
                FarmLocation = c.LandParcel.Farm.Location,
                InvestmentGoal = c.OpenInvestmentCycle.ExpectedFinancialGoal,
                CurrentInvestment = c.OpenInvestmentCycle.CurrentTotalInvestment,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                CropType = c.Crop.CropType.Name,
                CropName = c.Crop.Name,
                CropId= c.CropId,
            }).ToList();
        }

        public async Task<List<VisitedCyclesDTO>> GetVisitedCyclesAsync(int investorId)
        {
            var x = (await _unitOfWork.InteractionOfInvestor.FindAllWithIncludes<InteractionOfInvestor>(
                v => v.InvestorId == investorId
                )).ToList();
            var visited=new List<VisitedCyclesDTO>();
            foreach (var visit in x)
            {
                var v = new VisitedCyclesDTO
                {
                    CycleId = visit.CycleId,
                    CycleName=(await _cycleService.GetCycleByIdAsync(visit.CycleId)).CycleName,
                };
                visited.Add(v);
            }
            return visited;
        }
        public async Task<List<FavoriteFarmersDTO>> GetFavoriteFarmersAsync(int investorId)
        {
            var id = (await _unitOfWork.Investor.GetByIdAsync(investorId)).ApplicationUserId;
            var farmers= await _unitOfWork.FavoriteFarmers.FindAllAsync( f=>f.userId==id);
            var favoritefarmers=new List<FavoriteFarmersDTO>();
            foreach (var item in farmers)
            {
              var info= await _farmerService.GetFarmerInfo((await _unitOfWork.Farmer.GetByAppUserIdAsync(item.FarmerId)).FarmerId);
                var x = new FavoriteFarmersDTO
                {
                    FarmerId = info.FarmerId,
                    FarmerName = info.Name,
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
                CycleName = i.Cycle.CycleName,
                InvestedAmount = i.RequestedAmount,
                ReturnType = i.Cycle.OpenInvestmentCycle.AvailableProfitTypes,
                FarmerName = i.Cycle.LandParcel.Farm.Farmer.ApplicationUser?.UserName ?? "Unknown",
                CropId = i.Cycle.CropId,
                CropName = i.Cycle.Crop.Name,
                
            }).ToList();
        }

        public async Task<RecommendationDataDTO> GetRecommendationDataAsync(int investorId)
        {
            var availableCycles = await GetAvailableCyclesAsync();
            var investorHistory = await GetInvestorHistoryAsync(investorId);
            var favoriteFarmers = await GetFavoriteFarmersAsync(investorId);
            var visitedCycles = await GetVisitedCyclesAsync(investorId);

            return new RecommendationDataDTO
            {
                Cycles = availableCycles,
                investorHistory = investorHistory,
                favoriteFarmers = favoriteFarmers,
                visitedCycles = visitedCycles
            };
        }
    }
}
