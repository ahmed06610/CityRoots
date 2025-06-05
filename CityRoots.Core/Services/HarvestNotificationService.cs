using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace CityRoots.Core.Services
{
    public class HarvestNotificationService : IHarvestNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IPurchaseRequestService _purchaseRequestService;
        private readonly HarvestNotificationLogService _harvestNotificationLogService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public HarvestNotificationService(IUnitOfWork unitOfWork, INotificationService notificationService, IPurchaseRequestService purchaseRequestService, HarvestNotificationLogService harvestNotificationLogService, IHubContext<NotificationHub> hubContext)
        {

            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _purchaseRequestService = purchaseRequestService;
            _harvestNotificationLogService = harvestNotificationLogService;
            _hubContext = hubContext;
        }

        //public async Task ControlHarvestNotification(int HarvestId)
        //{
        //    var Harvest = await _unitOfWork.Harvest.GetByIdAsync(HarvestId);
        //    if (Harvest is null)
        //        throw new Exception($"No Harvests With this Id {HarvestId}");
        //    var UserId = Harvest.Farmer.ApplicationUser.Id;



        //    if(! await _harvestNotificationLogService.HarvestNotificationhassent(HarvestId,HarvestNotificationType.FinishedYield)&& Harvest.Yield==0)
        //    {
        //        await NotifyFinishedYield(HarvestId, UserId);
        //        await _harvestNotificationLogService.logHarvestNotification(HarvestId, HarvestNotificationType.FinishedYield, "farmer");
        //    }
        //}


        public async Task notifyOnPurchaseRequest(int HarvestId, int merchantId, PurchaseRequest request)
        {
            var Harvest = await _unitOfWork.Harvest.FindTWithIncludes<Harvest>(HarvestId, "HarvestId",
                x => x.Crop,
                x => x.Farmer);
            var userId = Harvest.Farmer.ApplicationUserId;
            var investor = await _unitOfWork.Merchant.FindTWithIncludes<Merchant>(merchantId, "MerchantId",
                x => x.ApplicationUser);
            var merchantname = investor.ApplicationUser.Name;


            var formattedDate = request.RequestDate.ToString("dddd, HH", System.Globalization.CultureInfo.InvariantCulture);
            var content = $"طلب شراء من {merchantname} للمحصول {Harvest.Crop.Name} (رقم: {Harvest.HarvestId}) بكمية {request.RequestedAmount} وسعر {request.RequestedPrice} مع الملاحظات: \"{request.Notes}\" بتاريخ {formattedDate}";
            var notification = new CreateNotificationDTO
            {
                Type = "Harvest",
                Content = content,
                UserId = userId,
                AdditionalData = $"HarvestId : {HarvestId}"

            };
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);



        }
        //notify For FinishedYield
        public async Task NotifyFinishedYield(HarvestNotificationDto harvest)
        {
            var userId = harvest.userId;
            var content = $"المحصول {harvest.cropName} رقم {harvest.HarvestId} تم الانتهاء من كميته";

            var notification = new CreateNotificationDTO
            {
                Type = "Harvest",
                Content = content,
                UserId = userId,
                AdditionalData = $"HarvestId : {harvest.HarvestId}"

            };
            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
           // await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);



        }
        public async Task NotifyMerchantOfpurchaseResponseAsync(string farmerName, HarvestNotificationDto harvest, string status)
        {
            var content = status == PurchaseRequestStatus.مقبول.ToString() ?
               $"لقد تم قبول طلبك من قبل {farmerName} بشأن طلبك لشراء محصول {harvest.cropName} رقم {harvest.HarvestId}" :
                $"لقد تم رفض طلبك من قبل {farmerName} بشأن طلبك لشراء محصول   {harvest.cropName} رقم {harvest.HarvestId}";

            var userId = harvest.merchantId;
            var notification = new CreateNotificationDTO
            {
                Type = "Purchaserequest",
                Content = content,
                UserId = userId,
                AdditionalData = $"{{ \"HarvestId\": {harvest.HarvestId} }}"



            };
            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }

          //  await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);





        }
    }
}

