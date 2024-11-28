using AutoMapper;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class HarvestService:IHarvestService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private const string ImagesFolder = "uploads/landparcels";
        private readonly IHttpContextAccessor httpContextAccessor;


        public HarvestService(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService,IHttpContextAccessor httpContextAccessor) {
        this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
            this.httpContextAccessor = httpContextAccessor;
        
        }


        public async Task<AddHarvestDto> Add(AddHarvestDto harvest)
        {
            var addedharvest = mapper.Map<Harvest>(harvest);

            if(harvest.Image != null) { 
            addedharvest.ImageUrl=imageService.SaveImage(harvest.Image,ImagesFolder);
            }
            var loggedIn = httpContextAccessor.HttpContext?.User?.FindFirst("LoggedId")?.Value;
            if (!string.IsNullOrEmpty(loggedIn) && int.TryParse(loggedIn, out int id))
            {
                addedharvest.FarmerId = id; // Assign FarmerId
            }
            else
            {
                throw new Exception("FarmerId is missing or invalid.");
            }

            await unitOfWork.Harvest.AddAsync(addedharvest);
            await unitOfWork.CompleteAsync();
            return harvest;
        }

        public async Task Delete(int id)
        {
            var harvest=await unitOfWork.Harvest.GetByIdAsync(id);
            if (harvest is null)
            {
                throw new Exception($"There is no Harvests with this id {id}");
            }
            imageService.DeleteImage(harvest.ImageUrl);

            await unitOfWork.Harvest.DeleteAsync(harvest);
            await unitOfWork.CompleteAsync();
        }

        public async Task<HarvestDisplayDto> Get(int id)
        {
      

            var harvest = await unitOfWork.Harvest.GetWithInclude(id);
            if (harvest is null)
                throw new Exception($"There is no Harvests with this id {id}");
            return mapper.Map<HarvestDisplayDto>(harvest);
        }

        public async Task<IEnumerable<HarvestDisplayDto>> GetAll(string s=null)
        {

           var harvests = await unitOfWork.Harvest.GetAllWithIncludes(s); 
            return mapper.Map<IEnumerable<HarvestDisplayDto>>(harvests);
        }

        public async Task<UpdateHarvestDto> Update(UpdateHarvestDto updateharvest)
        {
            var harvest = await unitOfWork.Harvest.GetByIdAsync(updateharvest.HarvestId);
            if (harvest is null)
                throw new Exception($"There is no Harvests with this id {updateharvest.HarvestId}");
            mapper.Map(updateharvest, harvest);
            if (updateharvest.Image != null)
            {
                // Delete the old image
                imageService.DeleteImage(harvest.ImageUrl);

                // Save the new image
                harvest.ImageUrl = imageService.SaveImage(updateharvest.Image, ImagesFolder);
            }
            unitOfWork.Harvest.Update(harvest);
        
            await unitOfWork.CompleteAsync();
            return updateharvest;

        }
    }
}
