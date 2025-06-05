using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class RateNotificationService:IRateNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public RateNotificationService(IUnitOfWork unitOfWork,INotificationService notificationService,IHubContext<NotificationHub> hubContext) {
        _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

       

        public async Task NotifyOnRating(string userName, string farmerId, int rating)
        {
            var starsWord = rating == 1 ? "نجمة" : "نجوم";
            var content = $"قام المستخدم {userName} بتقييمك بـ {rating} {starsWord}.";

            var notification = new CreateNotificationDTO
            {
                Content = content,
                Type = "Rate",
                UserId = farmerId
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == farmerId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
            //    await _hubContext.Clients.User(farmerId).SendAsync("ReceiveNotification", notification);
        }

    }
}
