using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNotificationAsync(CreateNotificationDTO notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Content = notificationDto.Content,
                Type = notificationDto.Type,
                AdditionalData = notificationDto.AdditionalData,
                Date = DateTime.Now
            };

            await _unitOfWork.Notification.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId, bool onlyUnread = false)
        {
            var notifications = await _unitOfWork.Notification
                .FindAllAsync(n => n.UserId == userId && (!onlyUnread || !n.IsRead));

            return notifications.Select(n => new NotificationDTO
            {
                NotificationId = n.NotificationId,
                Content = n.Content,
                Type = n.Type,
                Date = n.Date,
                IsRead = n.IsRead,
                AdditionalData = n.AdditionalData
            });
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _unitOfWork.Notification.GetByIdAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _unitOfWork.CompleteAsync();
            }
        }



        
    }
}
