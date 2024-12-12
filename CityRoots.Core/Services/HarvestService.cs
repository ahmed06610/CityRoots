using AutoMapper;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Purchasereque;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;

namespace CityRoots.Core.Services
{
    public class HarvestService : IHarvestService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private const string ImagesFolder = "uploads/landparcels";
        private readonly IHttpContextAccessor httpContextAccessor;


        public HarvestService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
            this.httpContextAccessor = httpContextAccessor;

        }


        public async Task<AddHarvestDto> Add(AddHarvestDto harvest,int farmerid=1)
        {
            var addedharvest = mapper.Map<Harvest>(harvest);

            if (harvest.Image != null)
            {
                addedharvest.ImageUrl = imageService.SaveImage(harvest.Image, ImagesFolder);
            }
            //var loggedIn = httpContextAccessor.HttpContext?.User?.FindFirst("LoggedId")?.Value;
            //if (!string.IsNullOrEmpty(loggedIn) && int.TryParse(loggedIn, out int id))
            //{
            //    addedharvest.FarmerId = id; // Assign FarmerId
            //}
            //else
            //{
            //    throw new Exception("FarmerId is missing or invalid.");
            //}
            addedharvest.FarmerId = farmerid;
            await unitOfWork.Harvest.AddAsync(addedharvest);
            await unitOfWork.CompleteAsync();
            return harvest;
        }

        public async Task Delete(int id)
        {
            var harvest = await unitOfWork.Harvest.GetByIdAsync(id);
            if (harvest is null)
            {
                throw new Exception($"There is no Harvests with this id {id}");
            }
            imageService.DeleteImage(harvest.ImageUrl);

            await unitOfWork.Harvest.DeleteAsync(harvest);
            await unitOfWork.CompleteAsync();
        }

        public async Task<HarvestDtoForFarmer> Get(int id)
        {


            var harvest = await unitOfWork.Harvest.FindTWithIncludes<Harvest>(id, x => x.Crop);
            if (harvest is null)
                throw new Exception($"There is no Harvests with this id {id}");
            return mapper.Map<HarvestDtoForFarmer>(harvest);
        }

        public async Task<IEnumerable<HarvestDtoForFarmer>> GetAll(string s = null,int farmerid=0)
        {

            var harvests = await unitOfWork.Harvest.FindAllWithIncludes<Harvest>(x => (x.Crop.Name.Contains(s)||s==null)&&(x.FarmerId==farmerid||farmerid==0)
            ,x=>x.Crop
            , x=>x.Purchases);
            var _harvests = new List<HarvestDtoForFarmer>();
            foreach(var harvest in harvests)
                _harvests.Add(await CheckStatus(harvest));


            return _harvests;
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
        private async Task <HarvestDtoForFarmer> CheckStatus(Harvest harvest)
        {
          
            
                if (harvest.Yield == 0)
                    harvest.status = "نفذت الكميه";
                else
                {
                    if (harvest.Purchases.Count > 0)
                    {
                        harvest.status = "تحت الطلب";

                    }

                }
                unitOfWork.Harvest.Update(harvest);
                await unitOfWork.CompleteAsync();
            
            return mapper.Map<HarvestDtoForFarmer>(harvest);
        }

        public async Task<OnePurchaseRequestForHarvest> GetOnePurchaseRequestForHarvest(int Id)
        {
            var Request=await unitOfWork.Purchase.FindTWithIncludes<PurchaseRequest>(Id,x=>x.Merchant,
                x=>x.Merchant.ApplicationUser,
                x=>x.Harvest,
                x=>x.Harvest.Purchases
                );
            if (Request is null)
                throw new Exception($"No Requsets With This Id {Id}");
            return mapper.Map<OnePurchaseRequestForHarvest>(Request);

        }

        public async Task<IEnumerable<AllPurchasesRequestForHarvest>> GetAllPurchasesRequestForHarvest(int harvestId)
        {
            var Requests = await unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(x => x.HarvestId == harvestId);
            return mapper.Map<IEnumerable<AllPurchasesRequestForHarvest>>(Requests);
        }
    }
}