using CityRoots.Core.DTOs.Farmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IFarmerService
    {
        Task<FarmerInfoDTO> GetFarmerInfo(int id);
        Task<FarmerInfoDTO> UpdateFarmer(UpdateFarmerDTO model);
    }
}
