using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Purchaserequest;
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
    public class PurchaseRequestService : IPurchaseRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PurchaseRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PurchaseRequest> CreatePurchaseRequest(CreatePurchaseRrquest Request,int merchantId)
        {
            var harvest = await _unitOfWork.Harvest.GetByIdAsync(Request.HarvestId);
            if (harvest is null)
            {
                throw new Exception($"No harvests Wit this Id {Request.HarvestId}");
            }
            if (harvest.status != HarvestStatus.متاح.ToString())
                throw new Exception("لاتسطيع ارسال طلب شراء للمحصول لانه بالفعل انتهي");
            //if (harvest.Price < purchaseRrquest.RequestedPrice)
            //    throw new Exception("لا يمكنك إرسال طلب الشراء، لأن السعر المحدد للمحصول غير صالح أو أقل من السعر المطلوب.");
            if (harvest.Yield == 0 || harvest.Yield < Request.RequestedAmount)
                throw new Exception("لا يمكنك إرسال طلب الشراء، لأن الكمية المطلوبة أكبر من الكمية المتاحة أو المحصول غير متوفر حالياً.");
            var purchaseRequest = _mapper.Map<PurchaseRequest>(Request);
            purchaseRequest.RequestStatus = "قيد_الانتظار";
            purchaseRequest.RequestDate = DateTime.Now;
            purchaseRequest.MerchantId = merchantId;
           // purchaseRequest.RequestedPrice=(decimal)Request.RequestedAmount*harvest.Price;
            await _unitOfWork.Purchase.AddAsync(purchaseRequest);
            await _unitOfWork.CompleteAsync();
            purchaseRequest.Harvest = null;
            return purchaseRequest;


        }

        public async Task Delete(int RequestId)
        {
            var request = await _unitOfWork.Purchase.GetByIdAsync(RequestId);
            if (request is null) throw new Exception($"No requests with this Id {RequestId}");
            await _unitOfWork.Purchase.DeleteAsync(request);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PurchaseRequestDsiplay>> GetAllRequestsForHarvest(int HarvestId)
        {
            var requests = (await _unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(x => x.HarvestId == HarvestId,
                x => x.Harvest,
                x => x.Harvest.Crop,
               x => x.Merchant,
               x => x.Merchant.ApplicationUser)).ToList();
            return _mapper.Map<List<PurchaseRequestDsiplay>>(requests);
        }

        public async Task<List<PurchaseRequestDsiplay>> GetAllRequestsForMerchant(int MerchantId)
        {
            var requests = (await _unitOfWork.Purchase.FindAllWithIncludes<PurchaseRequest>(x => x.MerchantId == MerchantId,
                     x => x.Harvest,
                     x => x.Harvest.Crop,
                    x => x.Merchant,
                    x => x.Merchant.ApplicationUser)).ToList();
            return _mapper.Map<List<PurchaseRequestDsiplay>>(requests);
        }

        public async Task<PurchaseRequest> GetSpecificRequest(int RequestId)
        {
            var request = await _unitOfWork.Purchase.FindTWithIncludes<PurchaseRequest>(RequestId, "PurchaseRequestId",
                   x => x.Harvest,
                     x => x.Harvest.Crop,
                    x => x.Merchant,
                    x => x.Merchant.ApplicationUser);
            if (request is null)
                throw new Exception($"No requests with this Id {RequestId}");
            return _mapper.Map<PurchaseRequest>(request);
        }

        public async Task<HarvestNotificationDto> UpdateRequest(int requestId, string status)
        {
            var request = await _unitOfWork.Purchase.FindTWithIncludes<PurchaseRequest>(requestId, "PurchaseRequestId",
                x=>x.Merchant,
                x=>x.Harvest,
                x=>x.Harvest.Crop);
            
            
            
            if (request is null) throw new Exception($"No requests with this Id {requestId}");
            var _harvest = new HarvestNotificationDto();
            
            if (status == PurchaseRequestStatus.مقبول.ToString())
            {
                var harvest = await _unitOfWork.Harvest.FindTWithIncludes<Harvest>(request.HarvestId, "HarvestId",
                     x=>x.Farmer,
                     x=>x.Crop
                     );

                if (harvest.Yield<request.RequestedAmount)
                    throw new Exception("لا يمكنك قبول طلب الشراء، لأن الكمية المطلوبة أكبر من الكمية المتاحة.");


                harvest.Yield -= request.RequestedAmount;
                if (harvest.Yield <= 0)
                {
                    harvest.status = HarvestStatus.منتهي.ToString();

                }
                _unitOfWork.Harvest.Update(harvest);
                _harvest.status=harvest.status;
                _harvest.userId=harvest.Farmer.ApplicationUserId;
               
            }
            request.RequestStatus = status;
            _unitOfWork.Purchase.Update(request);
            await _unitOfWork.CompleteAsync();
            _harvest.cropName=request.Harvest.Crop.Name;
            _harvest.merchantId = request.Merchant.ApplicationUserId;
            _harvest.HarvestId = request.HarvestId;
            return _harvest;
        }
    }
}
