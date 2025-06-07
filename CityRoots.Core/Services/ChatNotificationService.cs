using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        public ChatNotificationService(IHubContext<NotificationHub> hubContext, INotificationService notificationService, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task NotifyTheUserAboutNewMessage(string ReciverId, string senderName)
        {
            var message = $"{senderName} قام بإرسال رسالة جديدة إليك";
            var notification = new CreateNotificationDTO
            {
                Content = message,
                UserId = ReciverId,
                Type = "المحادثات"

            };
            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == ReciverId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
            //   await _hubContext.Clients.User(ReciverId).SendAsync("ReceiveNotification", notification);

        }
    }
}
