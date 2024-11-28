using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.OpenInvestmentCycle;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IOpenInvestmentCycleService
    {
        Task<OpenInvestmentCycleDTO> CreateOpenInvestmentCycleAsync(CreateOpenInvestmentCycleDTO dto);
        Task<OpenInvestmentCycleDTO> UpdateOpenInvestmentCycleAsync(UpdateOpenInvestmentCycleDTO dto);
        Task<bool> DeleteOpenInvestmentCycleAsync(int id);
    }
}
