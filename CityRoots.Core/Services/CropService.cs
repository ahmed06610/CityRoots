using AutoMapper;
using CityRoots.Core.DTOs.Crop;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class CropService:ICropService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        public CropService(IUnitOfWork unitOfWork,IMapper mapper) {
        this._unitOfWork = unitOfWork;
            this.mapper = mapper;

        }

        public async Task<CropDisplayDto> Add(AddCropDto crop)
        {
            var addedcrop=mapper.Map<Crop>(crop);
            await _unitOfWork.Crop.AddAsync(addedcrop);
            await _unitOfWork.CompleteAsync();
            return mapper.Map<CropDisplayDto>(addedcrop);

        }

        public async Task Delete(int id)
        {
            var crop = await _unitOfWork.Crop.GetByIdAsync(id);
            if (crop is null)
                throw new Exception($"There is crop with this Id {id}");
            await _unitOfWork.Crop.DeleteAsync(crop);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<CropDisplayDto> Get(int id)
        {
            var crop=await _unitOfWork.Crop.GetByIdAsync(id);
            if (crop is null)
                throw new Exception($"There is crop with this Id {id}");
            return mapper.Map<CropDisplayDto>(crop);

        }

        public async Task<IEnumerable<CropDisplayDto>> GetAll()
        {
            var Crops=await _unitOfWork.Crop.GetAllAsync();
            if (Crops is null || !Crops.Any())
                throw new Exception("No crops Found");
            return mapper.Map<IEnumerable<CropDisplayDto>>(Crops);
            
            
        }

        public async Task<CropDisplayDto> Update(UpdateCropDto cropRequest)
        {
            var crop = await _unitOfWork.Crop.GetByIdAsync(cropRequest.CropId);
            if (crop is null)
                throw new Exception($"There is crop with this Id {cropRequest.CropId}");
              crop=mapper.Map(cropRequest, crop);
             _unitOfWork.Crop.Update(crop);
              await _unitOfWork.CompleteAsync();
              return mapper.Map<CropDisplayDto>(crop);
          



        }
    }
}
