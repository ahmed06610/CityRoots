using CityRoots.Core.DTOs.Farm;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IFarmService
    {
        Task<List<Farm>> GetAllFarmsAsync(int FarmerId);
        Task<Farm> GetFarmByIdAsync(int id);
        Task<FarmDTO> AddFarmAsync(CreateFarmDTO farm);
        Task<FarmDTO> UpdateFarmAsync(UpdateFarmDTO farm);
        Task<bool> DeleteFarmAsync(int id);
    }
}
