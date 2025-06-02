using CityRoots.Core.DTOs.Chat;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IChatNotificationService _chatNotificationService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ChatController(IChatService chatService,IChatNotificationService chatNotificationService,IBackgroundJobClient backgroundJobClient)
        {
            _chatService = chatService;
            _backgroundJobClient = backgroundJobClient;
            _chatNotificationService=chatNotificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO model)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId == null)
                return Unauthorized();
            var userName = User?.FindFirst("NameOfuser")?.Value;

            await _chatService.SendMessageAsync(senderId, model.ReceiverId, model.Message);
            _backgroundJobClient.Enqueue(() =>
            _chatNotificationService.NotifyTheUserAboutNewMessage(model.ReceiverId,userName));
            return Ok(new { Message = "Message sent successfully." });
        }

        [HttpGet("messages/{receiverId}")]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId == null)
                return Unauthorized();

            var messages = await _chatService.GetChatMessagesAsync(senderId, receiverId);
            return Ok(messages);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetChatUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var users = await _chatService.GetChatUsersAsync(userId);
            return Ok(users);
        }

        [HttpPost("mark-as-read/{receiverId}")]
        public async Task<IActionResult> MarkAsRead(string receiverId)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId == null)
                return Unauthorized();

            await _chatService.MarkMessagesAsReadAsync(senderId, receiverId);
            return Ok(new { Message = "Messages marked as read." });
        }
    }
}
