using CityRoots.Core.DTOs.Crop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ICropService
    {
        Task<IEnumerable<CropDisplayDto>> GetAll();
        Task<IEnumerable<CropDTO>> GetCrops(int CropTypeid);
        

            Task<CropDisplayDto> Get(int id);
        Task<CropDisplayDto> Update(UpdateCropDto cropDisplayDto);
        Task<CropDisplayDto> Add(AddCropDto cropDisplayDto);
        Task Delete(int id);

    }
}
