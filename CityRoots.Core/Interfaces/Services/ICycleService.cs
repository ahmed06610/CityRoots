using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ICycleService
    {
        Task<List<CycleForFarmerDTO>> GetAllCyclesForFarmersAsync(int CycleerId,bool ForFarmer);
        Task<List<CycleForBrowsing>> GetAllCyclesForInvestorsAsync(InvestorRecommendationResponseDTO Recommendation = null);
        Task<List<CycleForInvestorDTO>> GetAllPrivateCyclesForInvestor(int InvestorId);

        Task<CycleForFarmerDTO> GetCycleByIdAsync(int id);
        Task<CycleDetailsForInvestorDTO> GetCycleByIdForInvestorAsync(int Cycleid, int InvestorId);

        Task<CycleDTO> AddCycleAsync(CreateCycleDTO cycle);
        Task<CycleDTO> UpdateCycleAsync(UpdateCycleDTO cycle);
        Task<bool> DeleteCycleAsync(int id);

    }
}
