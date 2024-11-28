using CityRoots.Core.DTOs.Cycle;
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
        Task<List<CycleDTO>> GetAllCyclesAsync(int CycleerId);
        Task<CycleDTO> GetCycleByIdAsync(int id);
        Task<CycleDTO> AddCycleAsync(CreateCycleDTO cycle);
        Task<CycleDTO> UpdateCycleAsync(UpdateCycleDTO cycle);
        Task<bool> DeleteCycleAsync(int id);
    }
}
