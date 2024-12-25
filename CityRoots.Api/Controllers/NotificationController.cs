using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO notificationDto)
        {
            await _notificationService.CreateNotificationAsync(notificationDto);
            return Ok("Notification created successfully.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(string userId, [FromQuery] bool onlyUnread = false)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId, onlyUnread);
            return Ok(notifications);
        }

        [HttpPatch("{notificationId}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok("Notification marked as read.");
        }
    
}
}
