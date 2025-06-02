using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class ChatNotificationService:IChatNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ChatNotificationService(IHubContext<NotificationHub> hubContext, INotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task NotifyTheUserAboutNewMessage(string ReciverId, string senderName)
        {
            var message = $"{senderName} قام بإرسال رسالة جديدة إليك";
            var notification = new CreateNotificationDTO
            {
                Content = message,
                UserId = ReciverId,
                Type = "Chat"

            };
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(ReciverId).SendAsync("ReceiveNotification", notification);

        }
    }
}
