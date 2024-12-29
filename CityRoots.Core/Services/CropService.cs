using AutoMapper;
using CityRoots.Core.DTOs.Crop;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;

namespace CityRoots.Core.Services
{
    public class CropService:ICropService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private const string ImagesFolder = "uploads/Crops";
        public CropService(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService) {
        this._unitOfWork = unitOfWork;
            this.imageService = imageService;

            this.mapper = mapper;

        }

        public async Task<CropDisplayDto> Add(AddCropDto crop)
        {
            var addedcrop=mapper.Map<Crop>(crop);
            if (crop.Image != null)
            {
                addedcrop.ImageUrl = imageService.SaveImage(crop.Image, ImagesFolder);
            }
            await _unitOfWork.Crop.AddAsync(addedcrop);
            await _unitOfWork.CompleteAsync();
            return mapper.Map<CropDisplayDto>(addedcrop);

        }

        public async Task Delete(int id)
        {
            var crop = await _unitOfWork.Crop.GetByIdAsync(id);
            if (crop is null)
                throw new Exception($"There is crop with this Id {id}");
            imageService.DeleteImage(crop.ImageUrl);

            await _unitOfWork.Crop.DeleteAsync(crop);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<CropDisplayDto> Get(int id)
        {
            var crop=await _unitOfWork.Crop.FindTWithIncludes<Crop>(id, "CropId", c=>c.CropType);
            if (crop is null)
                throw new Exception($"There is crop with this Id {id}");
            return mapper.Map<CropDisplayDto>(crop);

        }

        public async Task<IEnumerable<CropDisplayDto>> GetAll()
        {
            var Crops = await _unitOfWork.Crop.FindAllWithIncludes<Crop>(null, x => x.CropType);
           
            return mapper.Map<IEnumerable<CropDisplayDto>>(Crops);
            
            
        }

        public async Task<CropDisplayDto> Update(UpdateCropDto cropRequest)
        {
            var crop = await _unitOfWork.Crop.GetByIdAsync(cropRequest.CropId);
            if (crop is null)
                throw new Exception($"There is crop with this Id {cropRequest.CropId}");
            if(cropRequest.Image is not null)
            {
                imageService.DeleteImage(crop.ImageUrl);
                crop.ImageUrl = imageService.SaveImage(cropRequest.Image, ImagesFolder);

            }
              crop=mapper.Map(cropRequest, crop);
             _unitOfWork.Crop.Update(crop);
              await _unitOfWork.CompleteAsync();
              return mapper.Map<CropDisplayDto>(crop);
          



        }
    }
}
