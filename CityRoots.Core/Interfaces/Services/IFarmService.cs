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
        Task<List<FarmDTO>> GetAllFarmsAsync(int FarmerId);
        Task<FarmDTO> GetFarmByIdAsync(int id);
        Task<FarmDTO> AddFarmAsync(CreateFarmDTO farm);
        Task<FarmDTO> UpdateFarmAsync(UpdateFarmDTO farm);
        Task<bool> DeleteFarmAsync(int id);
    }
}
