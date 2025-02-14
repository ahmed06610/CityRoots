using CityRoots.Core.DTOs.LandParcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ILandParcelService
    {
        Task<List<LandParcelDTO>> GetAllLandParcelsAsync(int FarmId=0);
        Task<List<LandParcelDTO>> GetAllLandParcelsofFarmerAsync(int FarmerId = 0);

        Task<LandParcelDTO> GetLandParcelByIdAsync(int id);
        Task<LandParcelDTO> AddLandParcelAsync(CreateLandParcelDTO createLandParcelDto);
        Task<LandParcelDTO> UpdateLandParcelAsync(UpdateLandParcelDTO updateLandParcelDto);
        Task<bool> DeleteLandParcelAsync(int id);
    }
}
