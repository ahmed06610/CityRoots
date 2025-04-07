using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId is null) return Unauthorized();
            await _notificationService.CreateNotificationAsync(notificationDto);
            return Ok("Notification created successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications( [FromQuery] bool onlyUnread = false)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
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
