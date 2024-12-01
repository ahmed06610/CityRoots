using CityRoots.Core.DTOs.CycleUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ICycleUpdateService
    {
        Task<CycleUpdateDTO> CreateCycleUpdateAsync(CreateCycleUpdateDTO dto);
        Task<CycleUpdateDTO> UpdateCycleUpdateAsync(UpdateCycleUpdateDTO dto);
        Task<IEnumerable<CycleUpdateDTO>> GetAllUpdatesByCycleIdAsync(int cycleId);
        Task<bool> DeleteCycleUpdateAsync(int id);
    }
}
