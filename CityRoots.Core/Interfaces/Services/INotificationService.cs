using CityRoots.Core.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(CreateNotificationDTO notificationDto);
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId, bool onlyUnread = false);
        Task MarkAsReadAsync(int notificationId);
    }
}
