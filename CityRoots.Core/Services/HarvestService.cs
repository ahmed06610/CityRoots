using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Purchasereque;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;

namespace CityRoots.Core.Services
{
    public class HarvestService : IHarvestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService imageService;
        private readonly IFarmerService _farmerService;
        private readonly ICycleUpdateService _cycleUpdateService;
        private const string ImagesFolder = "uploads/Harvests";
        private readonly IHttpContextAccessor httpContextAccessor;


        public HarvestService(IUnitOfWork _unitOfWork, IMapper _mapper, IImageService imageService, IHttpContextAccessor httpContextAccessor, ICycleUpdateService cycleUpdateService, IFarmerService farmerService)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
            this.imageService = imageService;
            this.httpContextAccessor = httpContextAccessor;
            _cycleUpdateService = cycleUpdateService;
            _farmerService = farmerService;
        }


        public async Task<AddHarvestDto> Add(AddHarvestDto harvest,int farmerid=1)
        {
            var addedharvest = _mapper.Map<Harvest>(harvest);

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
            await _unitOfWork.Harvest.AddAsync(addedharvest);
            await _unitOfWork.CompleteAsync();
            return harvest;
        }

        public async Task Delete(int id)
        {
            var harvest = await _unitOfWork.Harvest.GetByIdAsync(id);
            if (harvest is null)
            {
                throw new Exception($"There is no Harvests with this id {id}");
            }
            imageService.DeleteImage(harvest.ImageUrl);

            await _unitOfWork.Harvest.DeleteAsync(harvest);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<HarvestDtoForFarmer> Get(int id)
        {


            var harvest = await _unitOfWork.Harvest.FindTWithIncludes<Harvest>(id,"HarvestId", x => x.Crop,x=>x.Purchases);
            if (harvest is null)
                throw new Exception($"There is no Harvests with this id {id}");
            harvest.Purchases=await GetAllRequests(id);
            return _mapper.Map<HarvestDtoForFarmer>(harvest);
        }

        public async Task<IEnumerable<HarvestDtoForFarmer>> GetAll(string s = null,int farmerid=0)
        {

            var harvests = await _unitOfWork.Harvest.FindAllWithIncludes<Harvest>(x => (x.Crop.Name.Contains(s)||s==null)&&(x.FarmerId==farmerid||farmerid==0)
            ,x=>x.Crop
            ,x=>x.Purchases,
            x=>x.Cycle
           
            );
          
            var _harvests = new List<HarvestDtoForFarmer>();

            foreach (var harvest in harvests)
            { 
                var har = _mapper.Map<HarvestDtoForFarmer>(harvest);

               
                    var x = await GetAllPurchasesRequestForHarvest(harvest.HarvestId);
                    har.Purchases = x.ToList();
                
                if(harvest.Cycle is not null)
                {
                    har.IsHarvestConnectToCycle=true;
                    har.CycleId=harvest.CycleId;
                }
                if (x.Count() > 0)
                {
                    har.ReuestsCount = x.Count();
                    har.Status = HarvestStatue.تحت_الطلب.ToString();
                }
                else if(har.Yield != 0)
                {
                    har.ReuestsCount = 0;
                    har.Status = HarvestStatue.متاح.ToString();
                }
                    _harvests.Add(har);
            }
         

            return _harvests;
        }
        public async Task<List<HarvestForBrowsing>> GetAllHarvestsForMerchantsAsync(MerchantRecommendationResponseDTO Recommendation = null, int MerchantId = 0)
        {
            Expression<Func<Harvest, bool>> criteria = null;

            if (Recommendation != null && Recommendation.recommended_harvest_ids != null && Recommendation.recommended_harvest_ids.Any())
            {
                criteria = h => Recommendation.recommended_harvest_ids.Contains(h.HarvestId)&& h.status == HarvestStatue.متاح.ToString() || h.status == HarvestStatue.تحت_الطلب.ToString();
                MerchantId=Recommendation.merchant_id;
            }
            else
            {
                // Get All Available Harvests
                criteria = h => h.status == HarvestStatue.متاح.ToString() || h.status == HarvestStatue.تحت_الطلب.ToString();
            }

            var harvests = (await _unitOfWork.Harvest.FindAllWithIncludes<Harvest>(criteria,
                h => h.Farmer,
                h => h.Farmer.ApplicationUser,
                h => h.Crop,
                h => h.Crop.CropType,
                h => h.Cycle,
                h => h.Cycle.LandParcel,
                h => h.Cycle.LandParcel.Farm
              )).ToList();

            var harvestsDTOs = new List<HarvestForBrowsing>();

            foreach (var harvest in harvests)
            {
                var farmerId = harvest.Farmer.ApplicationUser.Id;
                var ratings = (await _unitOfWork.Rate.FindAllWithIncludes<Rate>(r => r.FarmerId == farmerId));
                var rate = ratings.Count() !=0 ? (int)ratings.Average(r => r.Rating) : 0;

                var harvestDto = _mapper.Map<HarvestForBrowsing>(harvest);
                harvestDto.Rate = rate;
                var x =await _unitOfWork.Purchase.FindAllAsync(p => p.HarvestId == harvest.HarvestId && p.MerchantId == MerchantId && p.RequestStatus == PurchaseStatus.مقبول.ToString()) ;
                if (x.Count()!=0)
                {
                    harvestDto.IsBuyer=true;
                }
               
                harvestsDTOs.Add(harvestDto);
            }
            return harvestsDTOs;
        }

        public async Task<HarvestDetailsForMerchantDTO> GetHarvestByIdForMerchantAsync(int harvestId, int merchantId)
        {
            var appuserofmerchant = (await _unitOfWork.Merchant.GetByIdAsync(merchantId)).ApplicationUserId;
            var harvestForMerchant = new HarvestDetailsForMerchantDTO();

            // Fetch harvest details along with farmer and land parcel details
            var harvest = await _unitOfWork.Harvest.FindTWithIncludes<Harvest>(
                harvestId, "HarvestId",
                h => h.Farmer,
                h => h.Farmer.ApplicationUser,
                h => h.Crop,
                h => h.Crop.CropType,
                h => h.Cycle
            );
            var appuserofFarmer = (await _unitOfWork.Farmer.GetByIdAsync(harvest.FarmerId)).ApplicationUserId;

            var farmerInfo = await _farmerService.GetFarmerInfo(harvest.FarmerId);
            var harvestDetails = _mapper.Map<HarvestDetailsDTO>(harvest);

            // Check if the merchant has requested to purchase this harvest
            var purchaseRequest = await _unitOfWork.Purchase.FindTWithExpression<PurchaseRequest>(
                pr => pr.MerchantId == merchantId && pr.HarvestId == harvestId
            );

            if (purchaseRequest is not null)
            {
                harvestForMerchant.PurchaseRequestId = purchaseRequest.PurchaseRequestId;

                if (purchaseRequest.RequestStatus == PurchaseStatus.مقبول.ToString())
                {
                    harvestForMerchant.IsMerchantBuyer = true;
                }
            }
           var x= await _unitOfWork.FavoriteFarmers.FindTWithExpression<FavoriteFarmers>( ff => (ff.userId ==appuserofmerchant) && (ff.FarmerId == appuserofFarmer));
            if (x is not null)
                farmerInfo.IsFarmerInFav = true;
            else
                farmerInfo.IsFarmerInFav = false;
            // Check if the request is under review
            harvestForMerchant.RequestReview = (purchaseRequest is not null && purchaseRequest.RequestStatus == PurchaseStatus.قيد_الانتظار.ToString());

            // Assign retrieved details to DTO
            harvestForMerchant.HarvestDetails = harvestDetails;
            harvestForMerchant.Farmer = farmerInfo;

            if(harvest.Cycle!= null)
            {
                harvestForMerchant.cycleUpdates = (await _cycleUpdateService.GetAllUpdatesByCycleIdAsync((int) harvest.CycleId)).ToList();
            }
            return harvestForMerchant;
        }


        public async Task<HarvestDtoForFarmer> Update(UpdateHarvestDto updateharvest)
        {
            var harvest = await _unitOfWork.Harvest.FindTWithIncludes<Harvest>(updateharvest.HarvestId, "HarvestId", x => x.Crop
   , x => x.Purchases,
   x => x.Cycle
           );
            if (harvest is null)
                throw new Exception($"There is no Harvests with this id {updateharvest.HarvestId}");
            if(updateharvest.Yield != harvest.Yield && updateharvest.Yield > 0)
                harvest.status=HarvestStatue.متاح.ToString();

            _mapper.Map(updateharvest, harvest);
            if (updateharvest.Image != null)
            {
                // Delete the old image
                imageService.DeleteImage(harvest.ImageUrl);

                // Save the new image
                harvest.ImageUrl = imageService.SaveImage(updateharvest.Image, ImagesFolder);
            }
            _unitOfWork.Harvest.Update(harvest);
            var har=_mapper.Map<HarvestDtoForFarmer>(harvest);
            har.ImageUrl = harvest.ImageUrl;
            var x = await GetAllPurchasesRequestForHarvest(harvest.HarvestId);
            har.Purchases = x.ToList();

            if (harvest.Cycle is not null)
            {
                har.IsHarvestConnectToCycle = true;
                har.CycleId = harvest.CycleId;
            }
            if (x.Count() > 0)
            {
                har.ReuestsCount = x.Count();
                har.Status = HarvestStatue.تحت_الطلب.ToString();
            }
            else if (har.Yield != 0)
            {
                har.ReuestsCount = 0;
                har.Status = HarvestStatue.متاح.ToString();
            }

            await _unitOfWork.CompleteAsync();
            return har;

        }
        private async Task <Harvest> CheckStatus(Harvest harvest)
        {
          
            
                if (harvest.Yield == 0)
                    harvest.status = HarvestStatue.نفذت_الكميه.ToString();
                else
                {
                    if (harvest.Purchases.Count > 0)
                    {
                        harvest.status = HarvestStatue.تحت_الطلب.ToString();

                    }

                }
                _unitOfWork.Harvest.Update(harvest);
                await _unitOfWork.CompleteAsync();
            
            return harvest;
        }

      


        public async Task<IEnumerable<AllPurchasesRequestForHarvest>> GetAllPurchasesRequestForHarvest(int harvestId)
        {
            var Requests = await _unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(x => x.HarvestId == harvestId &&x.RequestStatus == PurchaseStatus.قيد_الانتظار.ToString()
            , x=>x.Merchant,
            x=>x.Merchant.ApplicationUser
            ,x=>x.Harvest
            ,x => x.Harvest.Purchases
            
            );
            return _mapper.Map<IEnumerable<AllPurchasesRequestForHarvest>>(Requests);
        }
        private async Task<List<PurchaseRequest>> GetAllRequests(int harvestId)
        {
            var Requests = (await _unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(x => x.HarvestId == harvestId
           , x => x.Merchant,
           x => x.Merchant.ApplicationUser
          

           )).ToList();
            return Requests;
        }
    }
}