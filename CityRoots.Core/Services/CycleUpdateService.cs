using AutoMapper;
using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityRoots.Core.DTOs.LandParcel;

namespace CityRoots.Core.Services
{
    public class CycleUpdateService : ICycleUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private const string ImagesFolder = "uploads/cycleupdates";

        public CycleUpdateService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IEnumerable<CycleUpdateDTO>> GetAllUpdatesByCycleIdAsync(int cycleId)
        {
            var updates = await _unitOfWork.CycleUpdate.FindAllAsync(u => u.CycleId == cycleId);
            return updates.Select(_mapper.Map<CycleUpdateDTO>);
        }

        public async Task<CycleUpdateDTO> CreateCycleUpdateAsync(CreateCycleUpdateDTO dto)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(dto.CycleId);
            if (cycle == null) throw new KeyNotFoundException("Cycle not found.");

            var cycleUpdate = _mapper.Map<CycleUpdate>(dto);
            cycleUpdate.UpdateDate = DateTime.UtcNow;

            // Save image
            if (dto.Image != null)
            {
                cycleUpdate.ImageUrl = _imageService.SaveImage(dto.Image, ImagesFolder);
            }

            await _unitOfWork.CycleUpdate.AddAsync(cycleUpdate);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CycleUpdateDTO>(cycleUpdate);
        }

        public async Task<CycleUpdateDTO> UpdateCycleUpdateAsync(UpdateCycleUpdateDTO dto)
        {
            var cycleUpdate = await _unitOfWork.CycleUpdate.GetByIdAsync(dto.UpdateId);
            if (cycleUpdate == null) throw new KeyNotFoundException("CycleUpdate not found.");

            _mapper.Map(dto, cycleUpdate);

            // Update image
            if (dto.Image != null)
            {
                // Delete the old image
                _imageService.DeleteImage(cycleUpdate.ImageUrl);

                // Save the new image
                cycleUpdate.ImageUrl = _imageService.SaveImage(dto.Image, ImagesFolder);
            }

            _unitOfWork.CycleUpdate.Update(cycleUpdate);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CycleUpdateDTO>(cycleUpdate);
        }

        public async Task<bool> DeleteCycleUpdateAsync(int id)
        {
            var cycleUpdate = await _unitOfWork.CycleUpdate.GetByIdAsync(id);
            if (cycleUpdate == null) return false;

            // Delete the image
            _imageService.DeleteImage(cycleUpdate.ImageUrl);

            await _unitOfWork.CycleUpdate.DeleteAsync(cycleUpdate);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
