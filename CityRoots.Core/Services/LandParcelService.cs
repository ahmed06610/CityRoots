using AutoMapper;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class LandParcelService : ILandParcelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private const string ImagesFolder = "uploads/landparcels";

        public LandParcelService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<List<LandParcelDTO>> GetAllLandParcelsAsync(int FarmId=0)
        {
            var landParcels = (await _unitOfWork.LandParcel.FindAllWithIncludes<LandParcel>((l=>l.FarmId==FarmId || FarmId == 0),l=>l.Farm)).ToList();
            return _mapper.Map<List<LandParcelDTO>>(landParcels);
        }

        public async Task<LandParcelDTO> GetLandParcelByIdAsync(int id)
        {
            var landParcel = (await _unitOfWork.LandParcel.FindAllWithIncludes<LandParcel>(
                lp => lp.ParcelId == id,
                lp => lp.Farm
            )).FirstOrDefault();

            return landParcel == null ? null : _mapper.Map<LandParcelDTO>(landParcel);
        }

        public async Task<LandParcelDTO> AddLandParcelAsync(CreateLandParcelDTO createLandParcelDto)
        {
            var landParcel = _mapper.Map<LandParcel>(createLandParcelDto);

            // Save image
            if (createLandParcelDto.Image != null)
            {
                landParcel.ImageUrl = _imageService.SaveImage(createLandParcelDto.Image, ImagesFolder);
            }

            var createdLandParcel = await _unitOfWork.LandParcel.AddAsync(landParcel);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LandParcelDTO>(createdLandParcel);
        }

        public async Task<LandParcelDTO> UpdateLandParcelAsync(UpdateLandParcelDTO updateLandParcelDto)
        {
            var existingLandParcel = await _unitOfWork.LandParcel.GetByIdAsync(updateLandParcelDto.ParcelId);
            if (existingLandParcel == null) return null;

            // Update fields
            _mapper.Map(updateLandParcelDto, existingLandParcel);

            // Update image
            if (updateLandParcelDto.Image != null)
            {
                // Delete the old image
                _imageService.DeleteImage(existingLandParcel.ImageUrl);

                // Save the new image
                existingLandParcel.ImageUrl = _imageService.SaveImage(updateLandParcelDto.Image, ImagesFolder);
            }

            _unitOfWork.LandParcel.Update(existingLandParcel);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LandParcelDTO>(existingLandParcel);
        }

        public async Task<bool> DeleteLandParcelAsync(int id)
        {
            var landParcel = await _unitOfWork.LandParcel.GetByIdAsync(id);
            if (landParcel == null) return false;

            // Delete the image
            _imageService.DeleteImage(landParcel.ImageUrl);

            await _unitOfWork.LandParcel.DeleteAsync(landParcel);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
