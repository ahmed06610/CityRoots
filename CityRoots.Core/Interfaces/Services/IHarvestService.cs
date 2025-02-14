using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Purchasereque;
using CityRoots.Core.DTOs.Recommendation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IHarvestService
    {
        Task<IEnumerable<HarvestDtoForFarmer>> GetAll(string s=null,int farmerid=0);
        Task<List<HarvestForBrowsing>> GetAllHarvestsForMerchantsAsync(MerchantRecommendationResponseDTO Recommendation = null, int MerchantId=0);
        Task<HarvestDetailsForMerchantDTO> GetHarvestByIdForMerchantAsync(int harvestId, int merchantId);

        Task<HarvestDtoForFarmer> Get(int id);
        Task<AddHarvestDto> Add(AddHarvestDto harvest,int farmerid);
        Task<UpdateHarvestDto> Update(UpdateHarvestDto harvest);
        Task Delete(int id);
        Task<IEnumerable<AllPurchasesRequestForHarvest>> GetAllPurchasesRequestForHarvest(int harvestId);
    }
}
