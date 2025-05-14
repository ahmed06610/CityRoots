using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class FavoriteFarmerNotificationService: IFavoriteFarmerNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public FavoriteFarmerNotificationService(IHubContext<NotificationHub> hubContext,INotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;

            
        }

        public async Task NotifyOnFavoriteList(string userName, string farmerId)
        {
            var content = $"قام المستخدم {userName} بإضافتك إلى قائمة المزارعين المفضلين لديه!";
            var notification = new CreateNotificationDTO
            {
                Content = content,
                UserId = farmerId,
                Type = "FavoriteFarmer"
            };
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(farmerId).SendAsync("ReceiveNotification", notification);

        }
    }
}
