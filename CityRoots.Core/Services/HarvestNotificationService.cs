using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;

namespace CityRoots.Core.Services
{
    public class HarvestNotificationService : IHarvestNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IPurchaseRequestService _purchaseRequestService;
        private readonly HarvestNotificationLogService _harvestNotificationLogService;

        public HarvestNotificationService(IUnitOfWork unitOfWork, INotificationService notificationService, IPurchaseRequestService purchaseRequestService, HarvestNotificationLogService harvestNotificationLogService)
        {

            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _purchaseRequestService = purchaseRequestService;
            _harvestNotificationLogService = harvestNotificationLogService;
        }

        public async Task ControlHarvestNotification(int HarvestId)
        {
            var Harvest = await _unitOfWork.Harvest.GetByIdAsync(HarvestId);
            if (Harvest is null)
                throw new Exception($"No Harvests With this Id {HarvestId}");
            var UserId = Harvest.Farmer.ApplicationUser.Id;


            if (Harvest.Purchases.Any(x => x.RequestStatus.ToLower() == "pending"))


            {

                foreach (var request in Harvest.Purchases.Where(x => x.RequestStatus.ToLower() == "pending"))

                {
                    
                    var merchantName = (await _unitOfWork.Merchant.FindTWithIncludes<Merchant>(request.MerchantId, "MerchantId",
                        c => c.ApplicationUser

                        )).ApplicationUser.Name;
                    if (!await _harvestNotificationLogService.HarvestNotificationhassent(HarvestId, HarvestNotificationType.PurchaseRequest, request.PurchaseRequestId))
                    {
                        await notifyOnPurchaseRequest(HarvestId, UserId, merchantName, request);
                        await _harvestNotificationLogService.logHarvestNotification(HarvestId,HarvestNotificationType.PurchaseRequest,"farmer",request.PurchaseRequestId);
                    }
                }
            }
            if(! await _harvestNotificationLogService.HarvestNotificationhassent(HarvestId,HarvestNotificationType.FinishedYield)&& Harvest.Yield==0)
            {
                await NotifyFinishedYield(HarvestId, UserId);
                await _harvestNotificationLogService.logHarvestNotification(HarvestId, HarvestNotificationType.FinishedYield, "farmer");
            }
        }
    

    public async Task notifyOnPurchaseRequest(int HarvestId, string UserId, string merchantname, PurchaseRequest request)
    {
        var Harvest = await _unitOfWork.Harvest.GetByIdAsync(HarvestId);
        if (Harvest is null)
            throw new Exception($"No Harvests With this Id {HarvestId}");
        var formattedDate = request.RequestDate.ToString("dddd, HH", System.Globalization.CultureInfo.InvariantCulture);
        var content = $"طلب شراء من {merchantname}للمحصول{Harvest.Crop.Name} رقم المحصول{Harvest.HarvestId}بقيمة {request.RequestedAmount} بسعر {request.RequestedPrice}  بملاحظات {request.Notes} في {formattedDate}";
        var notification = new CreateNotificationDTO
        {
            Type = "Harvest",
            Content = content,
            UserId = UserId,
            AdditionalData = $"HarvestId : {HarvestId}"

        };
        await _notificationService.CreateNotificationAsync(notification);



    }
        //notify For FinishedYield
        public async Task NotifyFinishedYield(int HarvestId,string UserId)
        {
            var Harvest = await _unitOfWork.Harvest.GetByIdAsync(HarvestId);
            if (Harvest is null)
                throw new Exception($"No Harvests With this Id {HarvestId}");
            var content = $"المحصول {Harvest.Crop.Name} رقم {HarvestId} تم الانتهاء من كميته";
            var notification = new CreateNotificationDTO
            {
                Type = "Harvest",
                Content = content,
                UserId = UserId,
                AdditionalData = $"HarvestId : {HarvestId}"

            };
            await _notificationService.CreateNotificationAsync(notification);


        }



}
}

